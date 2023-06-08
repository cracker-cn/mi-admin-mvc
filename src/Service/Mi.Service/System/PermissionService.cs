using System.Data;
using System.Linq;
using System.Security.Claims;

using Mi.Core.Factory;
using Mi.Core.Models.UI;
using Mi.Core.Service;
using Mi.Entity.System;
using Mi.Entity.System.Enum;
using Mi.IRepository.BASE;
using Mi.IService.System.Models.Result;
using Mi.Repository.BASE;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly CaptchaFactory _captchaFactory;
        private readonly HttpContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IMiUser _miUser;

        public PermissionService(MessageModel message
            , IPermissionRepository permissionRepository
            , IUserRepository userRepository
            , IRoleRepository roleRepository
            , IFunctionService functionService
            , CreatorFactory creatorFactory
            , CaptchaFactory captchaFactory
            , IHttpContextAccessor httpContextAccessor
            , IMemoryCache memoryCache
            , IMiUser miUser)
        {
            _message = message;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _functionService = functionService;
            _creatorFactory = creatorFactory;
            _captchaFactory = captchaFactory;
            _context = httpContextAccessor.HttpContext!;
            _memoryCache = memoryCache;
            _miUser = miUser;
        }

        public async Task<List<PaMenuModel>> GetSiderMenuAsync()
        {
            var topLevels = _functionService.GetFunctionsCache()
                .Where(x => _miUser.FuncIds.Contains(x.Id) && x.Node == EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu).OrderBy(x => x.Sort);
            var list = topLevels.Select(x => new PaMenuModel(x.Id, 0, x.FunctionName, x.Url,x.Icon, GetPaChildren(x.Id).ToList())).ToList();

            return await Task.FromResult(list);
        }

        private IList<PaMenuModel> GetPaChildren(long id)
        {
            var children = _functionService.GetFunctionsCache().Where(x => _miUser.FuncIds.Contains(x.Id) && x.Node != EnumTreeNode.RootNode && x.FunctionType == EnumFunctionType.Menu && x.ParentId == id).OrderBy(x => x.Sort);
            return children.Select(x => new PaMenuModel(x.Id, GetMenuType(x.Children),x.FunctionName,x.Url,x.Icon, GetPaChildren(x.Id).ToList())).ToList();
        }

        /// <summary>
        /// 获取菜单类型 0目录 1菜单
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private int GetMenuType(string? children)
        {
            if (string.IsNullOrEmpty(children)) return 1;
            var arr = children.Split(',');
            if (arr == null) return 1;
            var flag = _functionService.GetFunctionsCache().Any(x => arr.Contains(x.Id.ToString()) && x.FunctionType == EnumFunctionType.Menu);
            return flag ? 0 : 1;
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
            if (!userName.RegexValidate("[A-Za-z0-9]{4,12}")) return _message.Fail("用户名只支持大小写字母和数字，最短4位，最长12位");
            var count = _userRepository.ExecuteScalar<int>("select count(*) from SysUser where LOWER(UserName)=@name and IsDeleted=0", new { name = userName.ToLower() });
            if (count > 0) return _message.Fail("用户名已存在");

            var user = _creatorFactory.NewEntity<SysUser>();
            user.UserName = userName;
            user.PasswordSalt = EncryptionHelper.GetPasswordSalt();
            user.Password = EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            await _userRepository.AddAsync(user);

            return _message.Success("注册成功，请等待管理员审核！");
        }

        public async Task<MessageModel> LoginAsync(string userName, string password, string verifyCode)
        {
            var mac = StringHelper.GetMacAddress();
            if (!_captchaFactory.Validate(mac, verifyCode)) return _message.Fail("验证码错误");

            var user = _userRepository.Get(x => x.UserName.ToLower() == userName.ToLower());
            if (user == null) return _message.Fail("用户名不存在");
            if (user.IsEnabled == 0) return _message.Fail("没有登录权限，请联系管理员");

            var flag = user.Password == EncryptionHelper.GenEncodingPassword(password, user.PasswordSalt);
            if (!flag) return _message.Fail("用户名或密码错误");

            var claims = new Claim[]
            {
                new (ClaimTypes.Name,user.UserName),
                new (ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
            await QueryUserModelCacheAsync(user.Id, user.UserName);

            return _message.Success("登录成功");
        }

        public async Task<UserModel> QueryUserModelCacheAsync(long id, string userName)
        {
            var key = userName + "_info";
            var cacheData = _memoryCache.Get<UserModel>(key);
            if (cacheData != null) return cacheData;
            var user = await _userRepository.GetAsync(id);
            if (user != null)
            {
                var userModel = new UserModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSuperAdmin = user.IsSuperAdmin == 1
                };
                var roleFuncRepo = DotNetService.Get<IRepositoryBase<SysRoleFunction>>();
                var userRoleRepo = DotNetService.Get<IRepositoryBase<SysUserRole>>();

                var exp = ExpressionCreator.New<SysFunction>();
                if (userModel.IsSuperAdmin)
                {
                    userModel.Roles = "SuperAdmin";
                    exp = x => true;
                }
                else
                {
                    var roleIds = (await userRoleRepo.GetAllAsync(x => x.UserId == user.Id)).Select(x => x.RoleId).ToList();
                    userModel.Roles = string.Join(",", _roleRepository.GetAll(x => roleIds.Contains(x.Id)).Select(x => x.RoleName));
                    var funcIds = (await roleFuncRepo.GetAllAsync(x => roleIds.Contains(x.RoleId))).Select(x => x.FunctionId).ToList();
                    exp = x => funcIds.Contains(x.Id);
                }
                userModel.PowerItems = _functionService.GetFunctionsCache().Where(exp.Compile()).Select(x => new PowerItem
                {
                    Id = x.Id,
                    Url = x.Url,
                    AuthCode = x.AuthorizationCode
                }).ToList();

                _memoryCache.Set(key, userModel);
                return userModel;
            }

            return new UserModel();
        }

        public async Task<MessageModel> SetRoleFunctionsAsync(long id, IList<long> funcIds)
        {
            var role = _roleRepository.Get(id);
            if (role == null || role.Id <= 0) return _message.Fail("角色不存在");

            var repo = DotNetService.Get<IRepositoryBase<SysRoleFunction>>();
            await repo.ExecuteAsync("delete from SysRoleFunction where RoleId=@id", new { id });

            var powers = new List<SysRoleFunction>();
            foreach (var item in funcIds)
            {
                var temp = _creatorFactory.NewEntity<SysRoleFunction>();
                temp.RoleId = id;
                temp.FunctionId = item;
                powers.Add(temp);
            }

            if (powers.Count > 0) await repo.AddManyAsync(powers);

            return _message.Success();
        }

        public async Task LogoutAsync()
        {
            var key = _miUser.UserName + "_info";
            _memoryCache.Remove(key);
            await _context.SignOutAsync();
        }

        public async Task<MessageModel<IList<long>>> GetRoleFunctionIdsAsync(long id)
        {
            var roleFuncRepo = DotNetService.Get<IRepositoryBase<SysRoleFunction>>();
            var ids = (await roleFuncRepo.GetAllAsync(x => x.RoleId == id)).Select(x => x.FunctionId).ToList();

            return new MessageModel<IList<long>>(ids);
        }
    }
}