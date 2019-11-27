using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Application
{
    public class HttpRequest
    {
        private readonly HttpClient http;

        public HttpRequest()
        {
            http = new HttpClient();
            http.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        ~HttpRequest()
        {
            http.Dispose();
        }

        public async Task<T> GetAsync<T>(Uri uri)
        {
            var response = await http.GetAsync(uri);

            using(Stream stream = await response.Content.ReadAsStreamAsync())
            using(StreamReader sr = new StreamReader(stream))
            {
                string json = await sr.ReadToEndAsync();

                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver() { 
                        NamingStrategy = new SnakeCaseNamingStrategy() 
                    }
                });
            }
        }
    }
}
