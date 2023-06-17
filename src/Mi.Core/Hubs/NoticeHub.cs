﻿using Mi.Core.CommonOption;
using Mi.Core.DB;

using Microsoft.AspNetCore.SignalR;

namespace Mi.Core.Hubs
{
    /// <summary>
    /// 消息中心，自动发送通知； 
    /// 禁止从中心以外使用。
    /// </summary>
    public class NoticeHub : Hub
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task SendMessage(string userId, string title, string content)
        {
            var msg = DapperDB.QueryFirstOrDefault<Option>($"select Title as Name,Content as Value from SysMessage where IsDeleted=0 and Readed=0 and ReceiveUser='{userId}' order by CreatedOn asc limit 1;");
            if (msg != null)
            {
                title = msg.Name!;
                content = msg.Value!;
            }
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;
            await Clients.All.SendAsync("ReceiveMessage", title, content);
        }
    }
}
