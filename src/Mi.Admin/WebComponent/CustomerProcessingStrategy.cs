using AspNetCoreRateLimit;

using Mi.Core.Service;
using Mi.Core.Toolkit.Helper;
using Mi.Entity.System.Enum;
using Mi.IService.System;

namespace Mi.Admin.WebComponent
{
    public class CustomerProcessingStrategy : ProcessingStrategy
    {
        private readonly AsyncKeyLockProcessingStrategy _asyncLock;

        public CustomerProcessingStrategy(IRateLimitCounterStore counterStore, IRateLimitConfiguration config)
            : base(config)
        {
            _asyncLock = new AsyncKeyLockProcessingStrategy(counterStore, config);
        }

        public override async Task<RateLimitCounter> ProcessRequestAsync(ClientRequestIdentity requestIdentity, RateLimitRule rule, ICounterKeyBuilder counterKeyBuilder, RateLimitOptions rateLimitOptions, CancellationToken cancellationToken = default)
        {
            var functionService = DotNetService.Get<IFunctionService>();
            var pageUrls = functionService.GetFunctionsCache().Where(x => x.FunctionType == EnumFunctionType.Menu && x.Url != null).Select(x => x.Url!.ToLower());
            if (pageUrls.Any(x => x == requestIdentity.Path)) return new RateLimitCounter { Count = 0, Timestamp = TimeHelper.LocalTime() };

            return await _asyncLock.ProcessRequestAsync(requestIdentity, rule, counterKeyBuilder, rateLimitOptions);
        }
    }
}
