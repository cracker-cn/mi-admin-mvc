using System.Web;

using Mi.Core.Models;
using Mi.Core.Toolkit.Helper;

using Newtonsoft.Json;

using UAParser;

namespace Mi.Admin.WebComponent.Middleware
{
    public class MiHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly static string MIHEADER = MiHeader.MIHEADER;
        public MiHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"];
            var info = Parser.GetDefault().Parse(userAgent);
            var model = new MiHeader
            {
                Browser = info.UA.Family + info.UA.Major,
                System = info.OS.Family + info.OS.Major
            };
            if (!string.IsNullOrWhiteSpace(context.Request.Headers[MIHEADER]))
            {
                var str = EncryptionHelper.Base64Decode(context.Request.Headers[MIHEADER]!);
                if (str != null)
                {
                    var miHeader = HttpUtility.UrlDecode(str).Split("&");
                    if (miHeader.Length < 2) return;
                    model.Region = miHeader[0];
                    model.Ip = miHeader[1];
                }
            }
            context.Items.Add(MIHEADER, JsonConvert.SerializeObject(model));
            await _next(context);
        }
    }
}
