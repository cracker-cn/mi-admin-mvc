using Microsoft.Extensions.DependencyInjection;

namespace Mi.Toolkit.ICore
{
    public interface IServiceRegistrar
    {
        /// <summary>
        /// 配置注册
        /// </summary>
        /// <param name="service">服务</param>
        void ConfigService(IServiceCollection service);
    }

    /// <summary>
    /// Scoped方式注入
    /// </summary>
    public interface IScoped
    {
    }

    /// <summary>
    /// Singleton方式注入
    /// </summary>
    public interface ISingleton
    {
    }

    /// <summary>
    /// Transient方式注入
    /// </summary>
    public interface ITransient
    {
    }
}