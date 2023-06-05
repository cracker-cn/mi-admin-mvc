using Mi.Core.Toolkit.API;
using Mi.IRepository.BASE;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.Repository
{
    public class ServiceRegister : IServiceRegistrar
    {
        public void ConfigService(IServiceCollection service)
        {
            service.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            service.AddScoped(typeof(Repository<>));
        }
    }
}