using System;

namespace TestSolution.Data
{
    /// <summary>
    /// Задача
    /// </summary>
    public class Task : SystemBase
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Ответственный
        /// </summary>
        public string Responsible { get; set; }
        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Executor { get; set; }
    }
}
