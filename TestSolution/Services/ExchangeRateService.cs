using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TestSolution.Interfaces;

namespace TestSolution.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        public readonly string OverThreadExceptionMessage = "Десять потоков уже считывают файл!";
        public readonly string RateFileName = "current_rate.json";
        private readonly int _threadsMaxCount = 10;
        private readonly int _sleepTime = 300000;

        private int _currentNumberOfThreads = 0;
        public int CurrentNumberOfThreads => _currentNumberOfThreads;

        private readonly IHostingEnvironment _environment;

        public ExchangeRateService(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> GetExchangeRate()
        {
            if (Interlocked.CompareExchange(ref _currentNumberOfThreads, 0, 0) < _threadsMaxCount)
            {
                Interlocked.Increment(ref _currentNumberOfThreads);

                using (var reader = new StreamReader($"{_environment.ContentRootPath}\\{RateFileName}"))
                {
                    Thread.Sleep(_sleepTime);

                    string rate = await reader.ReadToEndAsync();

                    Interlocked.Decrement(ref _currentNumberOfThreads);

                    return rate;
                }
            }
            else
            {
                throw new Exception(OverThreadExceptionMessage);
            }
        }
    }
}
