using Mi.Toolkit.Helper;
using Mi.Toolkit.ICore;

namespace Mi.Toolkit.Extension
{
    /// <summary>
    /// 自动服务注册
    /// </summary>
    public static class ServiceRegisterExtension
    {
        /// <summary>
        /// 注册系统服务，使用继承注册器<see cref="IServiceRegistrar" />或<see cref="IScoped" />或<see cref="ITransient" />或
        /// <see cref="ISingleton" />
        /// </summary>
        /// <param name="service"></param>
        public static void AutoInject(this IServiceCollection service)
        {
            var aseArray = RuntimeHelper.GetAllAssemblies();
            var t1 = typeof(IServiceRegistrar);
            var t2 = typeof(IScoped);
            var t3 = typeof(ITransient);
            var t4 = typeof(ISingleton);
            var typeList = new List<Type> { t1, t2, t3, t4 };
            foreach (var ase in aseArray)
            {
                var list = (from type in ase.GetTypes()
                            where typeList.Any(t => t.IsAssignableFrom(type)) && !type.IsAbstract && type.IsClass
                            select type).ToList();
                list.ForEach(x =>
                {
                    if (x.IsInterface) return;
                    if (x.IsAssignableTo(t1))
                    {
                        var obj = Activator.CreateInstance(x);
                        x.GetMethod("ConfigService")!.Invoke(obj, new object?[] { service });
                    }
                    else
                    {
                        var implTypes = x.GetInterfaces().Where(x1 => !typeList.Contains(x1));
                        foreach (var implType in implTypes)
                        {
                            if (x.IsAssignableTo(t2))
                                service.AddScoped(implType, x);
                            if (x.IsAssignableTo(t3))
                                service.AddTransient(implType, x);
                            if (x.IsAssignableTo(t4))
                                service.AddSingleton(implType, x);
                        }
                    }
                });
            }
        }
    }
}