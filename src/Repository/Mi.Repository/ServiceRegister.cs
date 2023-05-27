using Mi.IRepository.BASE;
using Mi.Repository.System;

using Microsoft.Extensions.DependencyInjection;

namespace Mi.Repository
{
	public class ServiceRegister : IServiceRegistrar
	{
		public void ConfigService(IServiceCollection service)
		{
			service.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
		}
	}
}