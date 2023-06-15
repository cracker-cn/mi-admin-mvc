using Mi.Core.Extension;
using Mi.Core.Models;

using Microsoft.AspNetCore.Http;

namespace Mi.Core.GlobalUser
{
    public class MiUser : IMiUser
    {
        private readonly UserModel _user;
        public MiUser(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext.GetUser();
        }

        public long UserId => _user.UserId;

        public string UserName => _user.UserName;

        public bool IsSuperAdmin => _user.IsSuperAdmin;

        public IList<long> FuncIds
        {
            get
            {
                if (_user.PowerItems == null) return new List<long>();
                return _user.PowerItems.Select(x => x.Id).ToList();
            }
        }
    }
}