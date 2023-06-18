using Mi.Entity.System;
using Mi.IService.System.Models.Result;

namespace Mi.IService.System
{
	public interface IMessageService
	{
		Task<MessageModel> ReadedAsync(IList<long> msgIds);

		Task<MessageModel<PagingModel<SysMessage>>> GetMessageListAsync(MessageSearch search);

		Task<IList<HeaderMsg>> GetHeaderMsgAsync();
	}
}
