namespace Mi.IService.System
{
    public interface IUserService
    {
        Task<MessageModel> GetUserListAsync();
    }
}