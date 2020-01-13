using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Crayon.ExchangeRates.BusinessModel
{
    public class ApiResult
    {
        public Dictionary<string, Dictionary<string, JToken>> Rates { get; set; }
    }
}
