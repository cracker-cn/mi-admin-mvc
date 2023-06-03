using Mi.Core.Factory;
using Mi.Core.Models.UI;
using Mi.Entity.System.Enum;
using Mi.IService.System.Models.Result;

namespace Mi.Service.System
{
    public class PermissionService : IPermissionService, IScoped
    {
        private readonly MessageModel _message;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IFunctionService _functionService;
        private readonly CreatorFactory _creatorFactory;

        public PermissionService(MessageModel message
            , IPermissionRepository permissionRepository
            , IUserRepository userRepository
            , IRoleRepository roleRepository
            , IFunctionService functionService
            , CreatorFactory creatorFactory)
        {
            _message = message;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _functionService = functionService;
            _creatorFactory = creatorFactory;
        }

        public async Task<List<PaMenuModel>> GetSiderMenuAsync()
        {
            var topLevels = _functionService.GetFunctionsCache()
                .Where(x => x.Node == EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu).OrderBy(x => x.Sort);
            var list = topLevels.Select(x => new PaMenuModel(x.Id, 0, x.FunctionName, x.Url, GetPaChildren(x.Id).ToList())).ToList();

            return await Task.FromResult(list);
        }

        private IList<PaMenuModel> GetPaChildren(long id)
        {
            var children = _functionService.GetFunctionsCache().Where(x => x.Node != EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new PaMenuModel(x.Id, 1, x.FunctionName, x.Url, GetPaChildren(x.Id).ToList())).ToList();
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

        public async Task<MessageModel> RegisterAsync(string userName, string password)
        {
            if (userName.RegexValidate("[A-Za-z0-9]{4,12}"))
            {
                return _message.Fail("用户名只支持大小写字母和数字，最短4位，最长12位");
            }
            var user = _creatorFactory.NewEntity<SysUser>();
            user.UserName = userName;
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(password,user.PasswordSalt);

            await _userRepository.AddAsync(user);

            return _message.Success("注册成功，请等待管理员审核！");
        }

        public Task<MessageModel> LoginAsync(string userName, string password, string verifyCode)
        {
            throw new NotImplementedException();
        }
    }
}