using Mi.IRepository.System.QueryModel;

namespace Mi.IRepository.System
{
    public interface IUserRepository
    {
        Task<PagingModel<UserItem>> QueryListAsync(PagingSearchModel model);
    }
}