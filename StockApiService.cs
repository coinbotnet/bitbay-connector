using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbot.Bitbay.Models;
using Coinbot.Domain.Contracts;
using Coinbot.Domain.Contracts.Models;
using Coinbot.Domain.Contracts.Models.StockApiService;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Linq;
using AutoMapper;

namespace Coinbot.Bitbay
{
    public class StockApiService : IStockApiService
    {
        private readonly string _serviceUrl = "https://api.bitbay.net/rest/";
        private readonly string _apiOkResult = "Ok";
        private readonly string _apiFailResult = "Fail";
        private readonly IMapper _mapper;

        public StockApiService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<ServiceResponse<Transaction>> GetOrder(string baseCoin, string targetCoin, string apiKey, string secret, string orderRefId)
        {
            throw new NotImplementedException();
        }

        public StockInfo GetStockInfo()
        {
            return new StockInfo
            {
                FillOrKill = true
            };
        }

        public async Task<ServiceResponse<Tick>> GetTicker(string baseCoin, string targetCoin)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_serviceUrl);
                var response = await client.GetAsync(string.Format("trading/ticker/{1}-{0}", baseCoin, targetCoin));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<TickDTO>(json);

                    if (deserialized.status == _apiOkResult)
                    {
                        return new ServiceResponse<Tick>(0, _mapper.Map<Tick>(deserialized));
                    }
                    else
                        throw new Exception(JsonConvert.SerializeObject(deserialized.errors));

                }
                else
                    return new ServiceResponse<Tick>((int)response.StatusCode, null, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ServiceResponse<Transaction>> PlaceBuyOrder(string baseCoin, string targetCoin, double stack, string apiKey, string secret, double rate, bool? testOnly = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_serviceUrl);

                var timestamp = Helpers.GetUnixTimeInSeconds().ToString();
                var reqUrl = string.Format(CultureInfo.InvariantCulture, "trading/offer/{1}-{0}",
                    baseCoin,
                    targetCoin
                );

                var body = JsonConvert.SerializeObject(new TransactionToMadeDTO
                {
                    rate = rate.ToString("0.00000000", CultureInfo.InvariantCulture),
                    amount = (stack / rate).ToString("0.00", CultureInfo.InvariantCulture),
                    offerType = "BUY",
                    mode = "limit",
                    fillOrKill = this.GetStockInfo().FillOrKill
                });

                client.DefaultRequestHeaders.Add("API-Key", apiKey);
                client.DefaultRequestHeaders.Add("API-Hash", Helpers.GetHashSHA512(apiKey + timestamp + body, secret));
                client.DefaultRequestHeaders.Add("operation-id", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("Request-Timestamp", timestamp);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, reqUrl);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<TransactionMadeDTO>(json);

                    if (deserialized.status == _apiOkResult)
                    {
                        return new ServiceResponse<Transaction>(0, _mapper.Map<Transaction>(deserialized));
                    }
                    else
                        throw new Exception(JsonConvert.SerializeObject(deserialized.errors));
                }
                else
                    return new ServiceResponse<Transaction>((int)response.StatusCode, null, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ServiceResponse<Transaction>> PlaceSellOrder(string baseCoin, string targetCoin, double stack, string apiKey, string secret, double qty, double toSellFor, double? raisedChangeToSell = null, bool? testOnly = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_serviceUrl);

                var timestamp = Helpers.GetUnixTimeInSeconds().ToString();
                var reqUrl = string.Format(CultureInfo.InvariantCulture, "trading/offer/{1}-{0}",
                    baseCoin,
                    targetCoin
                );

                var body = JsonConvert.SerializeObject(new TransactionToMadeDTO
                {
                    rate = raisedChangeToSell == null ? toSellFor.ToString("0.00000000", CultureInfo.InvariantCulture) : raisedChangeToSell.Value.ToString("0.00000000", CultureInfo.InvariantCulture),
                    amount = qty.ToString("0.00", CultureInfo.InvariantCulture),
                    offerType = "SELL",
                    mode = "limit",
                    fillOrKill = this.GetStockInfo().FillOrKill
                });

                client.DefaultRequestHeaders.Add("API-Key", apiKey);
                client.DefaultRequestHeaders.Add("API-Hash", Helpers.GetHashSHA512(apiKey + timestamp + body, secret));
                client.DefaultRequestHeaders.Add("operation-id", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("Request-Timestamp", timestamp);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, reqUrl);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<TransactionMadeDTO>(json);

                    if (deserialized.status == _apiOkResult)
                    {
                        return new ServiceResponse<Transaction>(0, _mapper.Map<Transaction>(deserialized));
                    }
                    else
                        throw new Exception(JsonConvert.SerializeObject(deserialized.errors));
                }
                else
                    return new ServiceResponse<Transaction>((int)response.StatusCode, null, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
