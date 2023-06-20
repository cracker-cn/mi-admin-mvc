namespace Mi.Core.Models.WxWork
{
    public class WxAccessToken : WxApiResponseBase
    {
        public string? access_token { get; set; }

        /// <summary>
        /// 单位秒
        /// </summary>
        public int expires_in { get; set; }
    }
}
