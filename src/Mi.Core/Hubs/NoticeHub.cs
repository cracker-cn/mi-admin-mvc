using Mi.Core.CommonOption;
using Mi.Core.DB;
using Mi.Core.GlobalUser;
using Mi.Core.Service;

using Microsoft.AspNetCore.SignalR;

namespace Mi.Core.Hubs
{
	public class NoticeHub : Hub
	{
		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <param name="disabledAutoSend">true禁用自动发送，手动调用时必须</param>
		/// <returns></returns>
		public async Task SendMessage(string title, string content, bool disabledAutoSend = false)
		{
			if (!disabledAutoSend)
			{
				var user = DotNetService.Get<IMiUser>();
				var msg = DapperDB.QueryFirstOrDefault<Option>($"select Title as Name,Content as Value from SysMessage where IsDeleted=0 and Readed=0 and ReceiveUser='{user.UserId}' order by CreatedOn asc limit 1;");
				if(msg != null)
				{
					title = msg.Name!;
					content = msg.Value!;
				}
			}
			if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;
			await Clients.All.SendAsync("ReceiveMessage", title, content);
		}
	}
}
