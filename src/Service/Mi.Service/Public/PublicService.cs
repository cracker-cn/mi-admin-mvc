using Mi.Core.Abnormal;
using Mi.Core.Hubs;
using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.IRepository.BASE;
using Mi.IService.Public;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Mi.Service.Public
{
    public class PublicService : IPublicService, IScoped
    {
        private readonly PaConfigModel _uiConfig;
        private readonly IHubContext<NoticeHub> _noticeHub;
        private readonly IMiUser _miUser;
        private readonly MessageModel _msg;
        private readonly IDictService _dictService;

        public PublicService(IOptionsMonitor<PaConfigModel> uiConfig, IDictService dictService
            , IHubContext<NoticeHub> noticeHub
            , IMiUser miUser
            , MessageModel msg)
        {
            _dictService = dictService;
            _uiConfig = uiConfig.CurrentValue;
            _noticeHub = noticeHub;
            _miUser = miUser;
            _msg = msg;
        }

        public async Task<bool> WriteMessageAsync(string title, string content, IList<long> receiveUsers)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) throw new FriendlyException("消息的标题和内容不能为空");
            if (receiveUsers == null || receiveUsers.Count == 0) throw new FriendlyException("接收消息用户不能为空");

            var repo = DotNetService.Get<IRepositoryBase<SysMessage>>();
            var now = TimeHelper.LocalTime();
            var list = receiveUsers.Select(x => new SysMessage { Title = title, Content = content, Readed = 0, CreatedBy = _miUser.UserId, CreatedOn = now, ReceiveUser = x }).ToList();
            await repo.AddManyAsync(list);

            return true;
        }

        public PaConfigModel ReadConfig()
        {
            return _uiConfig;
        }

        public async Task<MessageModel> SetUiConfigAsync(SysConfigModel operation)
        {
            await _dictService.SetAsync(operation);
            return _msg.Success();
        }

        public async Task<MessageModel<SysConfigModel>> GetUiConfigAsync()
        {
            var config = await _dictService.GetAsync<SysConfigModel>("UiConfig");
            return _msg.Success("查询成功").As(config);
        }
    }
}