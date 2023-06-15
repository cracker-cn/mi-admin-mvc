using Mi.Core.DB;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mi.Core.Service
{
	public class SeedDataBackgroundService : BackgroundService
	{
		private readonly ILogger<SeedDataBackgroundService> _logger;
		public SeedDataBackgroundService(ILogger<SeedDataBackgroundService> logger)
		{
			_logger = logger;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("DB Initializing......");
			var flag = await DataInitialization.RunAsync();
			if (flag)
			{
				_logger.LogInformation("DB Initialization Compeleted!");
			}
            else
            {
				_logger.LogError("DB Initialization Failed!");
			}
        }
	}
}
