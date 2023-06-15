using Microsoft.AspNetCore.SignalR;

namespace Mi.Admin.Hubs
{
	public class NoticeHub : Hub
	{
		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <param name="disabledAutoSend">禁用自动发送，手动调用时必须</param>
		/// <returns></returns>
		public async Task SendMessage(string title, string content, bool disabledAutoSend = false)
		{
			await Clients.All.SendAsync("ReceiveMessage", title, content);
		}
	}
}
