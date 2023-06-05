using Mi.Entity.System;

namespace Mi.IService.System
{
    public interface IRoleService
    {
        Task<MessageModel> AddRoleAsync(string name, string remark);

        Task<MessageModel> RemoveRoleAsync(long id);

        Task<MessageModel<PagingModel<SysRole>>> GetRoleListAsync(RoleSearch search);

        Task<MessageModel> UpdateRoleAsync(long id, string name, string remark);

        Task<MessageModel<SysRole>> GetRoleAsync(long id);
    }
}