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

        /// <summary>
        /// 发送请求，body为json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="corpSecret"></param>
        /// <param name="httpMethod"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<T> SendAsync<T>(string url, string corpSecret, HttpMethod httpMethod, Dictionary<string, string>? param = null) where T : WxApiResponseBase
        {
            var message = new HttpRequestMessage(httpMethod, await ConcatTokenAsync(url, corpSecret));
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

        public Task<T> SendAsync<T, TParam>(string url, string corpSecret, HttpMethod httpMethod, TParam param) where T : WxApiResponseBase
        {
            var dict = RuntimeHelper.ParseDictionary(param);
            return SendAsync<T>(url, corpSecret, httpMethod, dict);
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="corpSecret"></param>
        /// <param name="queryString">拼接好的querystring参数，不用问号</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<T> GetAsync<T>(string url, string corpSecret, string queryString)
        {
            queryString = queryString.TrimStart('?');
            if (!queryString.StartsWith('&')) queryString += "&";
            using (var httpClient = _httpClientFactory.CreateClient(WxWorkConst.NAME))
            {
                var resMessage = await httpClient.GetAsync(await ConcatTokenAsync(url, corpSecret) + queryString);
                object? res;
                if(typeof(T).FullName == typeof(string).FullName)
                {
                    res = await resMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    res = await resMessage.Content.ReadFromJsonAsync<T>();
                }
                if (res == null) throw new ArgumentNullException(nameof(res), "GetAsync获取json为空");
                return (T)Convert.ChangeType(res,typeof(T));
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
            var url = "gettoken" + StringHelper.ParamString(dict);
            using (var httpClient = _httpClientFactory.CreateClient(WxWorkConst.NAME))
            {
                var resMessage = await httpClient.GetAsync(url);
                var res = await resMessage.Content.ReadFromJsonAsync<WxAccessToken>();
                var token = res?.access_token ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _cache.Set(StringHelper.CorpSecretKey(corpSecret), token, CacheConst.WxWorkTokenExpire);
                }
                return token;
            }
        }

        private async Task<string> ConcatTokenAsync(string url, string corpSecret)
        {
            var token = await GetAccessTokenAsync(corpSecret);
            return $"{url}?access_token={token}";
        }
    }
}
