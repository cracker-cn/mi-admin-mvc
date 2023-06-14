using System.Reflection;

using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.IService.Public;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

namespace Mi.Service.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly IConfiguration _configuration;
        private readonly PaConfigModel _uiConfig;
        public PublicService(IConfiguration configuration, IOptionsMonitor<PaConfigModel> uiConfig)
        {
            _configuration = configuration;
            _uiConfig = uiConfig.CurrentValue;
        }

        public PaConfigModel ReadConfig()
        {
            return _uiConfig;
        }
    }
}