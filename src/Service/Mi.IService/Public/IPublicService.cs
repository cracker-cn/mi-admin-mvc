using Mi.Core.Models.UI;

namespace Mi.IService.Public
{
    public interface IPublicService
    {
        Task<PaConfigModel> ReadConfigAsync();

        Task<MessageModel> SetUiConfigAsync(SysConfigModel operation);

        Task<MessageModel<SysConfigModel>> GetUiConfigAsync();

        Task<bool> WriteMessageAsync(string title,string content,IList<long> receiveUsers);
    }
}