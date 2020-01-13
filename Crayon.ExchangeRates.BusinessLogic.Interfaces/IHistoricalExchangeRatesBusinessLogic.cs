using System;
using System.Threading.Tasks;

namespace Crayon.ExchangeRates.BusinessLogic.Interfaces
{
    public interface IHistoricalExchangeRatesBusinessLogic
    {
        Task<string> CalculateHistoricalExchangeRatesAsync(string baseCurr, string targetCurr, DateTime[] dates);
    }
}
