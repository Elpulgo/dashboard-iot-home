using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DashboardIotHome
{
    public class HttpWrapper : IDisposable
    {
        private HttpClient m_HttpClient;
        private readonly TimeSpan m_TimeOut = TimeSpan.FromSeconds(30);

        public HttpWrapper()
        {
            m_HttpClient = new HttpClient()
            {
                Timeout = m_TimeOut
            };

            m_HttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> PostAsync<T>(T data, Uri url)
        {
            var jsonContent = JsonConvert.SerializeObject(data);

            using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
            using (var response = await m_HttpClient.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<T> PostFormUrlEncodedAsync<T>(IEnumerable<KeyValuePair<string, string>> data, Uri url)
        {
            using (var encodedContent = new FormUrlEncodedContent(data))
            using (var response = await m_HttpClient.PostAsync(url, encodedContent))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public async Task<string> GetAsync(Uri url)
        {
            using (var response = await m_HttpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public void Dispose()
        {
            m_HttpClient.Dispose();
        }
    }
}
