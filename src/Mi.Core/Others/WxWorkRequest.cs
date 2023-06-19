using System.Net.Http.Json;
using System.Text;

using Mi.Core.Factory;
using Mi.Core.GlobalVar;
using Mi.Core.Models.WxWork;
using Mi.Core.Toolkit.Helper;

using Newtonsoft.Json;

namespace Mi.Core.Others
{
    public class WxWorkRequest
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MemoryCacheFactory _cache;

        public WxWorkRequest(IHttpClientFactory httpClientFactory, MemoryCacheFactory cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<T> SendAsync<T>(string url, string corpSecret, HttpMethod httpMethod, Dictionary<string, string>? param = null)
        {
            var message = new HttpRequestMessage(httpMethod, url + "?access_token=" + GetAccessTokenAsync(corpSecret));
            if (param != null)
            {
                var str = JsonConvert.SerializeObject(param);
                message.Content = new StringContent(str, Encoding.UTF8, "application/json");
            }
            using (var httpClient = _httpClientFactory.CreateClient(WxWorkConst.NAME))
            {
                var res = await httpClient.SendAsync(message);
                var result = await res.Content.ReadFromJsonAsync<T>();
                if (result == null) throw new ArgumentException("SendAsync获取结果为空", nameof(T));
                return result;
            }
        }

        private async Task<string> GetAccessTokenAsync(string corpSecret)
        {
            if (_cache.Exists(StringHelper.CorpSecretKey(corpSecret))) return _cache.Get<string>(StringHelper.CorpSecretKey(corpSecret)) ?? "";
            var dict = new Dictionary<string, string>()
            {
                {"corpid",WxWorkConst.corpid},
                {"corpsecret",corpSecret}
            };
            var url = "cgi-bin/gettoken" + StringHelper.ParamString(dict);
            using(var httpClient = _httpClientFactory.CreateClient(WxWorkConst.NAME)) 
            {
                var resMessage = await httpClient.GetAsync(url);
                var res = await resMessage.Content.ReadFromJsonAsync<WxWorkAccessToken>();
                var token = res?.access_token ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _cache.Set(StringHelper.CorpSecretKey(corpSecret), token, CacheConst.WxWorkTokenExpire);
                }
                return token;
            }
        }
    }
}
