using Mi.Core.Abnormal;
using Mi.Core.Hubs;
using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.IRepository.BASE;
using Mi.IService.Public;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Mi.Service.Public
{
	public class PublicService : IPublicService, IScoped
	{
		private readonly IConfiguration _configuration;
		private readonly PaConfigModel _uiConfig;
		private readonly IHubContext<NoticeHub> _noticeHub;
		private readonly IMiUser _miUser;
		public PublicService(IConfiguration configuration, IOptionsMonitor<PaConfigModel> uiConfig
			, IHubContext<NoticeHub> noticeHub
			, IMiUser miUser)
		{
			_configuration = configuration;
			_uiConfig = uiConfig.CurrentValue;
			_noticeHub = noticeHub;
			_miUser = miUser;
		}

		public async Task<bool> PushMessageAsync(string title, string content, IList<long> receiveUsers)
		{
			if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) throw new FriendlyException("消息的标题和内容不能为空");
			if (receiveUsers == null || receiveUsers.Count == 0) throw new FriendlyException("接收消息用户不能为空");

			var repo = DotNetService.Get<IRepositoryBase<SysMessage>>();
			var now = TimeHelper.LocalTime();
			var list = receiveUsers.Select(x => new SysMessage { Title = title, Content = content, Readed = 0, CreatedBy = _miUser.UserId, CreatedOn = now, ReceiveUser = x }).ToList();
			await repo.AddManyAsync(list);
			if(receiveUsers.Contains(_miUser.UserId)) await _noticeHub.Clients.All.SendAsync("ReceiveMessage",title,content);

			return true;
		}

		public PaConfigModel ReadConfig()
		{
			return _uiConfig;
		}
	}
}