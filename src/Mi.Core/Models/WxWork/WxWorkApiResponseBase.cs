namespace Mi.Core.Models.WxWork
{
    public class WxWorkApiResponseBase
    {
        public int errcode { get; set; }
        public string errmsg { get; set; } = "ok";
    }

    public static class WxWorkApiResponseBaseExtension
    {
        /// <summary>
        /// 检查 model != null && model.errcode == 0
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Succeed(this WxWorkApiResponseBase model)
        {
            return model != null && model.errcode == 0;
        }
    }
}
