using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TestSolution.Controllers;
using TestSolution.Data;
using TestSolution.Repositories;
using Xunit;

namespace XUnitTests
{
    /// <summary>
    /// Тесты для задач
    /// </summary>
    public class TaskTests
    {
        private readonly TasksController _tasksController;

        public TaskTests()
        {
            _tasksController = new TasksController(new TaskRepository());
        }

        /// <summary>
        /// Задача должна создаваться
        /// </summary>
        [Fact]
        public void ShouldCreateNewTask()
        {
            var testTask = MakeNew(0);
            var response = _tasksController.Create(testTask);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newTask = Assert.IsType<Task>(result.Value);
            Assert.True(newTask.Id > 0);
            Assert.Null(newTask.UpdateTime);
            Assert.Null(newTask.ArchiveTime);
            Assert.Equal(testTask.Title, newTask.Title);
            Assert.Equal(testTask.Responsible, newTask.Responsible);
            Assert.Equal(testTask.Executor, newTask.Executor);
        }

        /// <summary>
        /// Уже созданная задача должна возвращаться по её идентификатору
        /// </summary>
        [Fact]
        public void ShouldGetTaskById()
        {
            var testTask = MakeNew(1);
            var response = _tasksController.Create(testTask);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newTask = Assert.IsType<Task>(result.Value);

            response = _tasksController.Get(newTask.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var task = Assert.IsType<Task>(result.Value);
            Assert.Equal(newTask.Id, task.Id);
            Assert.Equal(testTask.Title, task.Title);
            Assert.Equal(testTask.Responsible, task.Responsible);
            Assert.Equal(testTask.Executor, task.Executor);
        }

        /// <summary>
        /// Существующая задача должна обновляться
        /// </summary>
        [Fact]
        public void ShouldUpdateTask()
        {
            var testTask = MakeNew(2);
            var response = _tasksController.Create(testTask);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newTask = Assert.IsType<Task>(result.Value);
            Assert.Null(newTask.UpdateTime);

            newTask.Title = "Updated task title";
            newTask.Responsible = "Updated responsible";
            newTask.Executor = "Updated executor";
            response = _tasksController.Update(newTask);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var updatedTask = Assert.IsType<Task>(result.Value);
            Assert.NotNull(updatedTask.UpdateTime);
            Assert.Equal(newTask.Id, updatedTask.Id);
            Assert.Equal(newTask.Title, updatedTask.Title);
            Assert.Equal(newTask.Responsible, updatedTask.Responsible);
            Assert.Equal(newTask.Executor, updatedTask.Executor);
        }

        /// <summary>
        /// Существующая задача должна отправляться в архив
        /// </summary>
        [Fact]
        public void ShouldArchiveTask()
        {
            var testTask = MakeNew(3);
            var response = _tasksController.Create(testTask);
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var newTask = Assert.IsType<Task>(result.Value);
            Assert.Null(newTask.ArchiveTime);

            response = _tasksController.Archive(newTask.Id);
            result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(result.Value);
            var archivedTask = Assert.IsType<Task>(result.Value);
            Assert.NotNull(archivedTask.ArchiveTime);
            Assert.Equal(newTask.Id, archivedTask.Id);
        }

        private Task MakeNew(int taskNumber)
        {
            return new Task
            {
                Title = $"TEST_TASK_{taskNumber}",
                Responsible = $"Test Responsible {taskNumber}",
                Executor = $"Test Executor {taskNumber}"
            };
        }
    }
}
