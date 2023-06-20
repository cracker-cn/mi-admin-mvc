using AspNetCoreRateLimit;

using Mi.Admin.WebComponent.Middleware;

namespace Mi.Admin.WebComponent
{
    public static class MiddlewareExtension
    {
        /// <summary>
        /// 加入自定义中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddCustomerMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<UserMiddleware>();
            builder.UseMiddleware<MiHeaderMiddleware>();
            //启用客户端IP限制速率
            builder.UseIpRateLimiting();
            return builder;
        }
    }
}
