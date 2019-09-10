using System;

namespace TestSolution.Data
{
    /// <summary>
    /// Документ
    /// </summary>
    public class Document : SystemBase
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Время первого прочтения
        /// </summary>
        public DateTime? FirstReadTime { get; set; }
    }
}
