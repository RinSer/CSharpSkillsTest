namespace TestSolution.Interfaces
{
    /// <summary>
    /// Базовый функционал для работы 
    /// с сущностями Задач и Документов
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Создать
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        TEntity Create(TEntity newEntity);
        /// <summary>
        /// Получить
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        TEntity Get(long entityId);
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        TEntity Update(TEntity updatedEntity);
        /// <summary>
        /// Отправить в архив
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        TEntity Archive(long entityId);
    }
}
