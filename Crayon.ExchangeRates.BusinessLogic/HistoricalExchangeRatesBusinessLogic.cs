using Crayon.ExchangeRates.BusinessLogic.Interfaces;
using Crayon.ExchangeRates.BusinessModel;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crayon.ExchangeRates.BusinessLogic
{
    public class HistoricalExchangeRatesBusinessLogic : IHistoricalExchangeRatesBusinessLogic
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;
        private static string HistoricalExchangeRateEntry => "_HistoricalExchangeRateEntry";

        public HistoricalExchangeRatesBusinessLogic(HttpClient httpClient, IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _distributedCache = distributedCache;
        }

        public async Task<string> CalculateHistoricalExchangeRatesAsync(string baseCurr, string targetCurr, DateTime[] dates)
        {
            var minDate = dates.Min();
            var maxDate = dates.Max();

            var validationPassed = ValidateInput(baseCurr, targetCurr, minDate, maxDate);

            if (validationPassed)
            {
                var responseString = _distributedCache.GetString(HistoricalExchangeRateEntry);

                if (string.IsNullOrEmpty(responseString))
                {
                    responseString = await _httpClient.GetStringAsync(
                        $"history?start_at={minDate:yyyy-MM-dd}&end_at={maxDate:yyyy-MM-dd}&base={baseCurr}&symbols={targetCurr}");

                    _distributedCache.SetString(HistoricalExchangeRateEntry, responseString);
                }

                var apiResult = JsonConvert.DeserializeObject<ApiResult>(responseString);

                return OutputResults(apiResult.Rates);
            }

            return "Please provide valid input data!";
        }

        private bool ValidateInput(string baseCurr, string targetCurr, DateTime minDate, DateTime maxDate)
        {
            switch (baseCurr)
            {
                case null:
                    return false;
                default:
                {
                    if (baseCurr.Length != 3)
                    {
                        return false;
                    }

                    break;
                }
            }

            switch (targetCurr)
            {
                case null:
                    return false;
                default:
                {
                    if (targetCurr.Length != 3)
                    {
                        return false;
                    }

                    break;
                }
            }

            if (minDate < new DateTime(1999, 1, 1))
            {
                return false;
            }

            if (maxDate < new DateTime(1999, 1, 1))
            {
                return false;
            }

            return true;
        }

        private string OutputResults(Dictionary<string, Dictionary<string, JToken>> rates)
        {
            var listOfDailyRatesOrdered = rates.
                Select(m => new DailyRate { Date = m.Key,
                    Amount = Convert.ToDouble(m.Value.Values.FirstOrDefault()) }).
                        OrderBy(m => m.Amount).ToList();

            var minRate = PrepareData(listOfDailyRatesOrdered, out var maxRate, out var avgRate);

            if (minRate != null && maxRate != null)
                return
                    $"A min rate of {minRate.Amount} on {minRate.Date}{Environment.NewLine}A max rate of {maxRate.Amount} on {maxRate.Date}{Environment.NewLine}An average rate of {avgRate}";
            else
                return string.Empty;
        }

        private static DailyRate PrepareData(List<DailyRate> listOfDailyRatesOrdered, out DailyRate maxRate, out double avgRate)
        {
            var minRate = listOfDailyRatesOrdered.FirstOrDefault();
            maxRate = listOfDailyRatesOrdered.LastOrDefault();
            avgRate = listOfDailyRatesOrdered.Select(m => m.Amount).Average();

            return minRate;
        }
    }
}
