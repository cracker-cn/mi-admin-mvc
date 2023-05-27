using Mi.Core.Models.Paging;
using Mi.IRepository.System;
using Mi.IRepository.System.QueryModel;

namespace Mi.Repository.System
{
    public class UserRepository : IUserRepository
    {
        public Task<PagingModel<UserItem>> QueryListAsync(PagingSearchModel model)
        {
            throw new NotImplementedException();
        }
    }
}