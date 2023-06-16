using Mi.Core.Models.UI;

namespace Mi.IService.Public
{
    public interface IPublicService
    {
        PaConfigModel ReadConfig();

        Task<MessageModel> SetUiConfigAsync(SysConfigModel operation);

        Task<MessageModel<SysConfigModel>> GetUiConfigAsync();

        Task<bool> WriteMessageAsync(string title,string content,IList<long> receiveUsers);
    }
}