using Mi.Core.Models.UI;

namespace Mi.IService.Public
{
    public interface IPublicService
    {
        PaConfigModel ReadConfig();

        Task<bool> WriteMessageAsync(string title,string content,IList<long> receiveUsers);
    }
}