using Mi.Core.GlobalVar;
using Mi.Core.Models.WxWork;
using Mi.Core.Service;
using Mi.IService.System.Models.Result;
using Mi.IService.WxWork.Models.Result;

namespace Mi.Application
{
    internal class ServiceRegister : IServiceRegistrar
    {
        public void ConfigService(IServiceCollection service)
        {
            service.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<SysDict, DictItem>();
                cfg.CreateMap<DictOperation, SysDict>();
                cfg.CreateMap<FunctionOperation, SysFunction>();
                cfg.CreateMap<WxUserItem, WxDeptUser>();
            });
            service.AddScoped<WxConfig>(p =>
            {
                var dictService = DotNetService.Get<IDictService>();
                return dictService.Get<WxConfig>(DictKeyConst.WxWork);
            });
        }
    }
}