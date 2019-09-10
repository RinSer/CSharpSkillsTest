using System.Threading.Tasks;

namespace TestSolution.Interfaces
{
    /// <summary>
    /// Сервис для получения обменного курса
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Получить обменный курс
        /// </summary>
        /// <returns></returns>
        Task<string> GetExchangeRate();
    }
}
