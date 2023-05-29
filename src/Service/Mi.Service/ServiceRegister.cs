using Mi.IService.System.Models.Result;

namespace Mi.Service
{
    internal class ServiceRegister : IServiceRegistrar
    {
        public void ConfigService(IServiceCollection service)
        {
            service.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<SysDict, SysDictItem>();
                cfg.CreateMap<DictOperation, SysDict>();
            });
        }
    }
}