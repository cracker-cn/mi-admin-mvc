using AspNetCoreRateLimit;

using Mi.Core.Toolkit.API;

namespace Mi.Admin.WebComponent
{
    public class ServiceRegister : IServiceRegistrar
    {
        public void ConfigService(IServiceCollection service)
        {
            var configuration = ConfigurationExtension.AppSettings;

            //从appsettings.json中加载常规配置，IpRateLimiting与配置文件中节点对应
            service.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            //从appsettings.json中加载Ip规则
            service.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            //注入计数器和规则存储
            service.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            service.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //配置（解析器、计数器密钥生成器）
            service.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            //4.0后必须注入处理策略
            service.AddSingleton<IProcessingStrategy, CustomerProcessingStrategy>();
        }
    }
}
