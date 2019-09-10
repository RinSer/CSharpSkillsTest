using System;

namespace TestSolution.Data
{
    /// <summary>
    /// Базовый класс с системными полями,
    /// общими для всех сущностей
    /// </summary>
    public abstract class SystemBase
    {
        /// <summary>
        /// Числовой идентификатор
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Время отправки в архив
        /// </summary>
        public DateTime? ArchiveTime { get; set; }
        /// <summary>
        /// Время последнего изменения
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
