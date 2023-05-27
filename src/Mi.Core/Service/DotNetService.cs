namespace Mi.Core.Service
{
    public class DotNetService
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public static IServiceProvider Instance { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public static TService Get<TService>() where TService : class
        {
            var service = (TService?)Instance.GetService(typeof(TService));
            if (service == null)
                throw new Exception($"获取失败，服务\"{typeof(TService).FullName}\"为空！");
            return service;
        }
    }
}