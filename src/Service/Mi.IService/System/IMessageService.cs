using Mi.Entity.System;

namespace Mi.IService.System
{
	public interface IMessageService
	{
		Task<MessageModel> ReadedAsync(IList<long> msgIds);

		Task<MessageModel<PagingModel<SysMessage>>> GetMessageListAsync(MessageSearch search);
	}
}
