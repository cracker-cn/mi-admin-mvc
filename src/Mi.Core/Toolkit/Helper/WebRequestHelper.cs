using System.Net.Http.Json;
using System.Text;

using Newtonsoft.Json;

namespace Mi.Core.Toolkit.Helper
{
    public class WebRequestHelper
    {
        public async static Task<string> GetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var content = client.Send(new HttpRequestMessage(HttpMethod.Get, url)).Content;
                return await content.ReadAsStringAsync();
            }
        }

        public static Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> param)
        {
            var str = string.Join('&', param.Select(x => $"{x.Key}={x.Value}"));
            str = "?" + str.Trim('&');
            throw new NotImplementedException();
        }

        public static Task<TResult> PostAsync<TResult>(string url, Dictionary<string, string> param)
        {
            throw new NotImplementedException();
        }

        public static async Task<TResult> SendAsync<TResult>(HttpMethod method, string url, Dictionary<string, string> param)
        {
            var str = JsonConvert.SerializeObject(param);
            var message = new HttpRequestMessage(method, url) { Content = new StringContent(str, Encoding.UTF8, "application/json") };
            using (var client = new HttpClient())
            {
                var res = await client.SendAsync(message);
                var result = await res.Content.ReadFromJsonAsync<TResult>();
                if (result == null) throw new ArgumentException("SendAsync为空", nameof(TResult));
                return result;
            }
        }
    }
}
