﻿using Mi.Core.Toolkit.API;
using Mi.IService.System.Models.Result;

namespace Mi.Service
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
            });
        }
    }
}