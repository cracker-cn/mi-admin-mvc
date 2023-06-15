using Mi.IRepository.BASE;

namespace Mi.Service.System
{
	public class MessageService : IMessageService,IScoped
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

		public async Task<MessageModel<PagingModel<SysMessage>>> GetMessageListAsync(MessageSearch search)
		{
			var exp = ExpressionCreator.New<SysMessage>()
				.AndIf(!string.IsNullOrEmpty(search.Title), x => x.Title.Contains(search.Title!));
			if (!string.IsNullOrEmpty(search.WriteTime) && search.WriteTime.Contains("~"))
			{
				var v1 = DateTime.TryParse(search.WriteTime.Split("~")[0], out var start);
				var v2 = DateTime.TryParse(search.WriteTime.Split("~")[1], out var end);
				if (v1 && v2) exp = exp.And(x => x.CreatedOn >= start && x.CreatedOn <= end);
			}

			var result = await _messageRepository.QueryPageAsync(search.Page, search.Size, exp);
			return new MessageModel<PagingModel<SysMessage>>(result);
		}

		public async Task<MessageModel> ReadedAsync(long msgId)
		{
			await _messageRepository.UpdateAsync(msgId, node => node.ModifiedTime().ModifiedUser(_miUser.UserId).Set(x => x.Readed, 1));
			return _msg.Success();
		}
	}
}
