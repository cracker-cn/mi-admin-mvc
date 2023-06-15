using Mi.Entity.System;

namespace Mi.IService.System
{
	public interface IMessageService
	{
		Task<MessageModel> ReadedAsync(long msgId);

		Task<MessageModel<PagingModel<SysMessage>>> GetMessageListAsync(MessageSearch search);
	}
}
