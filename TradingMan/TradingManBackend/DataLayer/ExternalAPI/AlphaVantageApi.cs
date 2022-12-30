using System.Net;
using TradingManBackend.Util;
using Newtonsoft.Json.Linq;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.DataLayer.ExternalAPI
{
    /// <summary>
    /// Class for communication with AlphaVantage API primarily for getting stock data.
    /// </summary>
    public class AlphaVantageApi
    {
        private const string FunctionTimeSeriesIntraday = "TIME_SERIES_INTRADAY";
        private const string FunctionTimeSeriesDaily = "TIME_SERIES_DAILY";
        private const string FucntionGlobalQuote = "GLOBAL_QUOTE";


        private readonly WebClient _client = new WebClient();

        /// <summary>
        /// Returns available end of day stock data for given symbol. Used to chart graphs.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<StockData> GetDailyData(string symbol)
        {
            string queryUrl = @$"https://www.alphavantage.co/query?function={FunctionTimeSeriesDaily}&symbol={symbol}&apikey={Constants.AlphaVantageApiKey}";
            Uri queryUri = new Uri(queryUrl);

            var jTokens = JObject.Parse(_client.DownloadString(queryUri)).GetValue("Time Series (Daily)");

            var data = new List<StockData>();
            foreach (var item in jTokens)
            {
                var children = item.Children();

                var dataEntry = new StockData()
                {
                    TimeStamp = DateTime.Parse(item.ToObject<JProperty>().Name),
                    Open = double.Parse(children["1. open"].First().Value<string>()),
                    High = double.Parse(children["2. high"].First().Value<string>()),
                    Low = double.Parse(children["3. low"].First().Value<string>()),
                    Close = double.Parse(children["4. close"].First().Value<string>()),
                    Volume = double.Parse(children["5. volume"].First().Value<string>()),
                };

                data.Add(dataEntry);
            }

            // Need to reverse order of elements for grap usage in frontend.
            data.Reverse();

            return data;
        }

        /// <summary>
        /// Returns current price of product for given symbol. Returns end of day price if requested after market closes.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public double GetCurrentPrice(string symbol)
        {
            string queryUrl = @$"https://www.alphavantage.co/query?function={FucntionGlobalQuote}&symbol={symbol}&apikey={Constants.AlphaVantageApiKey}";
            Uri queryUri = new Uri(queryUrl);

            var jObjects = JObject.Parse(_client.DownloadString(queryUri));
            return double.Parse(jObjects["Global Quote"]["05. price"].Value<string>());
        }
    }
}
