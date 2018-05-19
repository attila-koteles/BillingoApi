using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillingoApi.Core
{
    public class DataLoader<T>
    {
        Connection connection;
        public const int MaxPerPage = 50;
        readonly string serviceEndpoint;

        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();

        public DataLoader(Connection connection, string serviceEndpoint)
        {
            this.connection = connection;
            this.serviceEndpoint = serviceEndpoint;
        }

        private async Task<string> ReadJsonFromServiceEndpoint(int page, int limit)
        {
            if (limit > MaxPerPage) limit = MaxPerPage;

            var parameters = new List<string>
            {
                $"page={page}",
                $"max_per_page={limit}"
            };

            // Add our extra parameters
            foreach (var qp in QueryParameters)
                parameters.Add($"{qp.Key}={qp.Value}");

            return await connection.GetAsync($"{serviceEndpoint}?{string.Join("&", parameters)}");
        }

        private JsonSerializer GetCamelCaseSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(true, true)
                }
            };

            return JsonSerializer.CreateDefault(settings);
        }

        private List<T> ParseJsonToTypes(string json)
        {
            JObject jo = JObject.Parse(json);
            var tokens = jo.SelectTokens("$.data[*].attributes");
            var serializer = GetCamelCaseSerializer();
            return tokens.Select(token => token.ToObject<T>(serializer)).ToList();
        }

        public async Task<List<T>> FetchPageAsync(int page, int limit)
        {
            var json = await ReadJsonFromServiceEndpoint(page, limit);
            return ParseJsonToTypes(json);
        }

        public async Task<List<T>> FetchAllAsync(int limit)
        {
            var page = 0;
            var results = new List<T>();
            var remaining = limit;

            while (results.Count < limit)
            {
                page += 1;
                var fetchedItems = await FetchPageAsync(page, remaining);
                results.AddRange(fetchedItems);

                // This means we there are no more pages to fetch from the server
                if (fetchedItems.Count < MaxPerPage)
                    break;

                remaining = limit - results.Count;
            }

            return results;
        }
    }
}