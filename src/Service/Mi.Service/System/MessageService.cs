﻿using Dapper;

using Mi.IRepository.BASE;
using Mi.IService.System.Models.Result;

namespace Mi.Service.System
{
    public class MessageService : IMessageService, IScoped
    {
        private readonly IRepositoryBase<SysMessage> _messageRepository;
        private readonly IMiUser _miUser;
        private readonly MessageModel _msg;
        public MessageService(IRepositoryBase<SysMessage> messageRepository, IMiUser miUser, MessageModel msg)
        {
            _messageRepository = messageRepository;
            _miUser = miUser;
            _msg = msg;
        }

        public async Task<IList<HeaderMsg>> GetHeaderMsgAsync()
        {
            var list = (await _messageRepository.GetAllAsync(x => x.Readed == 0 && x.ReceiveUser == _miUser.UserId)).OrderByDescending(x => x.CreatedOn);
            var result = new List<HeaderMsg>();
            var msg = new HeaderMsg
            {
                Title = "未读消息",
                Id = 1,
                Children = list.Select(x => new HeaderMsgChild
                {
                    Id = x.Id,
                    Title = x.Title,
                    Context = x.Content,
                    Time = ShowTime(x.CreatedOn)
                }).ToList()
            };
            result.Add(msg);
            return result;
        }

        private string ShowTime(DateTime time)
        {
            var now = TimeHelper.LocalTime();
            var val = now.Subtract(time);
            if (val.TotalSeconds <= 60) return "刚刚";
            else if (val.TotalHours < 1) return $"{Math.Ceiling(val.TotalMinutes)}分钟前";
            else if (val.TotalHours >= 1 && val.TotalHours <= 23) return $"{Math.Ceiling(val.TotalHours)}小时前";
            else return $"{Math.Ceiling(val.TotalDays)}天前";
        }

        public async Task<MessageModel<PagingModel<SysMessage>>> GetMessageListAsync(MessageSearch search)
        {
            var sql = "select * from SysMessage where IsDeleted=0 and ReceiveUser=@userId";
            var parameter = new DynamicParameters();
            parameter.Add("userId",_miUser.UserId);
            if (!string.IsNullOrEmpty(search.Title))
            {
                sql += " and Title like @title";
                parameter.Add("title", "%" + search.Title + "%");
            }
            if(search.No.HasValue && search.No.Value > 0)
            {
                sql += " and Id=@id";
                parameter.Add("id", search.No.GetValueOrDefault());
            }
            if(search.Readed.HasValue && search.Readed >= 0)
            {
                sql += " and Readed=@readed";
                parameter.Add("readed", search.Readed.GetValueOrDefault());
            }
            if (!string.IsNullOrEmpty(search.WriteTime) && search.WriteTime.Contains("~"))
            {
                var v1 = DateTime.TryParse(search.WriteTime.Split("~")[0], out var start);
                var v2 = DateTime.TryParse(search.WriteTime.Split("~")[1], out var end);
                if(v1 && v2)
                {
                    sql += " and CreatedOn >= @start and CreatedOn <= @end";
                    parameter.Add("start", start.Date.ToString("yyyy-MM-dd"));
                    parameter.Add("end", end.Date.ToString("yyyy-MM-dd") + " 23:59:59");
                }
            }

            var result = await _messageRepository.QueryPageAsync(search.Page, search.Size, sql,parameter, " Readed asc,CreatedOn desc ");
            return new MessageModel<PagingModel<SysMessage>>(result);
        }

        public async Task<MessageModel> ReadedAsync(IList<long> msgIds)
        {
            if (msgIds.Count == 0) return _msg.ParameterError(nameof(msgIds));
            await _messageRepository.ExecuteAsync("update SysMessage set ModifiedOn=@time,ModifiedBy=@user,Readed=1 where Id in @ids", new { time = TimeHelper.LocalTime(), user = _miUser.UserId, ids = msgIds });
            return _msg.Success();
        }
    }
}
