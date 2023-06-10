using System.Security.Claims;

using Mi.Core.Extension;
using Mi.Core.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

namespace Mi.Core.GlobalUser
{
    public class MiUser : IMiUser
    {
        private readonly HttpContext _context;

        public MiUser(IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor.HttpContext!;
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

        private UserModel User => _context.GetUser();
    }
}