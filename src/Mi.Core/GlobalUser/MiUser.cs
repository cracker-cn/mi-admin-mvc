using System.Security.Claims;

using Mi.Core.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

namespace Mi.Core.GlobalUser
{
    public class MiUser : IMiUser
    {
        private readonly HttpContext _context;
        private readonly IMemoryCache _cache;

        public MiUser(IHttpContextAccessor contextAccessor, IMemoryCache cache)
        {
            _context = contextAccessor.HttpContext!;
            _cache = cache;
        }

        public long UserId => User.UserId;

        public string UserName => User.UserName;

        public bool IsSuperAdmin => User.IsSuperAdmin;

        public IList<long> FuncIds
        {
            get
            {
                if (User.PowerItems == null) return new List<long>();
                return User.PowerItems.Select(x => x.Id).ToList();
            }
        }

        private UserModel User => GetUser();

        private UserModel GetUser()
        {
            var userName = _context.User.FindFirst(ClaimTypes.Name)?.Value ?? "";
            return _cache.Get<UserModel>(userName + "_info") ?? new UserModel();
        }
    }
}