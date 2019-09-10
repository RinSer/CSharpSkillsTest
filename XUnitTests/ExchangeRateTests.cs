using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TestSolution.Controllers;
using TestSolution.Services;
using Xunit;

namespace XUnitTests
{
    /// <summary>
    /// Тест получения обменного курса
    /// </summary>
    public class ExchangeRateTests
    {
        /// <summary>
        /// Курс должен возвращаться только десяти параллельным потокам,
        /// если число единовременных потоков превышает десять,
        /// то все потоки кроме десяти выдают ошибку вместо курса.
        /// </summary>
        [Fact]
        public void ExchangeRateControllerTest()
        {
            var projectRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                .Location.Replace("XUnitTests\\bin\\Debug\\netcoreapp2.2", "TestSolution"));
            var mockEnvironment = new Mock<IHostingEnvironment>();

            mockEnvironment.SetupGet(m => m.ContentRootPath).Returns(projectRoot);

            var rateService = new ExchangeRateService(mockEnvironment.Object);
            var rateController = new ExchangeRateController(rateService);

            var testRate = File.ReadAllText($"{projectRoot}\\{rateService.RateFileName}");

            var okResultsCount = 0;
            var badResultsCount = 0;
            var maxTasks = 100;
            List<Task> tasks = new List<Task>();
            for (var i = 0; i < maxTasks; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var response = await rateController.Get();
                    var result = response.Result;

                    if (result is OkObjectResult okResult)
                    {
                        Assert.NotNull(okResult.Value);
                        Assert.Equal(testRate, okResult.Value);
                        Assert.InRange(rateService.CurrentNumberOfThreads, 0, 9);
                        Interlocked.Increment(ref okResultsCount);
                    }

                    if (result is BadRequestObjectResult badResult)
                    {
                        Assert.NotNull(badResult.Value);
                        Assert.Equal(rateService.OverThreadExceptionMessage, badResult.Value);
                        Assert.Equal(10, rateService.CurrentNumberOfThreads);
                        Interlocked.Increment(ref badResultsCount);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Assert.Equal(10, okResultsCount);
            Assert.Equal(maxTasks-10, badResultsCount);
        }

    }
}
