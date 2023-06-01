using Mi.Core.CommonOption;
using Mi.Core.Models.UI;
using Mi.Core.Toolkit.API;
using Mi.IService.System.Models.Result;

namespace Mi.Service.System
{
    public class PermissionService : IPermissionService, IScoped
    {
        private readonly MessageModel _message;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public PermissionService(MessageModel message
            , IPermissionRepository permissionRepository
            , IUserRepository userRepository
            , IRoleRepository roleRepository)
        {
            _message = message;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public Task<List<PaMenuModel>> GetSiderMenuAsync()
        {
            var menu = new List<PaMenuModel>
            {
                new PaMenuModel(0,"工作空间","",new List<PaMenuModel>
                {
                    new PaMenuModel(1,"仪表板","/Workspace/Dashboard")
                }),
                new PaMenuModel(0,"系统管理","",new List<PaMenuModel>
                {
                    new PaMenuModel(1,"用户管理","/System/User"),
                    new PaMenuModel(1,"角色管理","/System/Role"),
                    new PaMenuModel(1,"功能管理","/System/Function"),
                    new PaMenuModel(1,"数据字典","/System/Dict"),
                })
            };

            return Task.FromResult(menu);
        }

        public async Task<MessageModel<IList<UserRoleOption>>> GetUserRolesAsync(long userId)
        {
            var roles = await _roleRepository.GetAllAsync();
            var userRoles = await _permissionRepository.QueryUserRolesAsync(userId);
            var list = roles.Select(x => new UserRoleOption
            {
                Name = x.RoleName,
                Value = x.Id.ToString(),
                Checked = userRoles.Any(m => m.Id == x.Id),
                Remark = x.Remark
            }).ToList();

            return new MessageModel<IList<UserRoleOption>>(true, list);
        }

        public async Task<MessageModel> SetUserRoleAsync(long userId, List<long> roleIds)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null) return _message.Fail("用户不存在");

            await _permissionRepository.UserRoleRepo.ExecuteAsync("delete from SysUserRole where UserId=@id", new { id = userId });
            var list = new List<SysUserRole>();
            foreach (var roleId in roleIds)
            {
                list.Add(new SysUserRole
                {
                    Id = IdHelper.SnowflakeId(),
                    CreatedBy = userId,
                    CreatedOn = TimeHelper.LocalTime(),
                    UserId = userId,
                    RoleId = roleId
                });
            }
            await _permissionRepository.UserRoleRepo.AddManyAsync(list);

            return _message.Success();
        }
    }
}