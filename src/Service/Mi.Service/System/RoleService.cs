using Mi.Core.Toolkit.API;

namespace Mi.Service.System
{
    public class RoleService : IRoleService, IScoped
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMiUser _miUser;
        private readonly MessageModel _message;

        public RoleService(IRoleRepository roleRepository, IMiUser miUser, MessageModel message)
        {
            _roleRepository = roleRepository;
            _miUser = miUser;
            _message = message;
        }

        public async Task<MessageModel> AddRoleAsync(string name, string remark)
        {
            var isExist = (await _roleRepository.GetAllAsync(x=>x.RoleName.ToLower() == name.ToLower())).Count>0;
            if (isExist) return _message.Fail("角色名已存在");

            var role = new SysRole
            {
                Id = IdHelper.SnowflakeId(),
                RoleName = name,
                Remark = remark,
                CreatedBy = _miUser.UserId,
                CreatedOn = TimeHelper.LocalTime()
            };
            await _roleRepository.AddAsync(role);

            return _message.Success();
        }

        public async Task<MessageModel<SysRole>> GetRoleAsync(long id)
        {
            var role = await _roleRepository.GetAsync(id);

            return new MessageModel<SysRole>(true, role);
        }

        public async Task<MessageModel<PagingModel<SysRole>>> GetRoleListAsync(RoleSearch search)
        {
            var exp = ExpressionCreator.New<SysRole>()
                .AndIf(!string.IsNullOrEmpty(search.RoleName), x => x.RoleName.Contains(search.RoleName!));

            var pageModel = await _roleRepository.QueryPageAsync(search.Page, search.Size, exp);

            return new MessageModel<PagingModel<SysRole>>(true, "查询成功", pageModel);
        }

        public async Task<MessageModel> RemoveRoleAsync(long id)
        {
            await _roleRepository.UpdateAsync(id, node => node.MarkDeleted()
                .ModifiedUser(_miUser.UserId).ModifiedTime());

            return _message.Success();
        }

        public async Task<MessageModel> UpdateRoleAsync(long id, string name, string remark)
        {
            var isExist = (await _roleRepository.GetAllAsync(x => x.RoleName.ToLower() == name.ToLower())).Count > 0;
            var role = await _roleRepository.GetAsync(id);
            if (isExist && role.RoleName != name) return _message.Fail("角色名已存在");

            await _roleRepository.UpdateAsync(id, node => node.Set(x => x.RoleName, name)
                .ModifiedTime()
                .Set(x => x.Remark, remark)
                .ModifiedUser(_miUser.UserId));

            return _message.Success();
        }
    }
}