using Mi.Core.Models.UI;

namespace Mi.IService.Public
{
    public interface IPublicService
    {
        PaConfigModel ReadConfig();

        Task<bool> PushMessageAsync(string title,string content);
    }
}