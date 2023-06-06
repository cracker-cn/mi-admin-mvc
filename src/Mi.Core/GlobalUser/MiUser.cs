using System.Security.Claims;

using Mi.Core.Models;

using Microsoft.AspNetCore.Http;

namespace Mi.Core.GlobalUser
{
	public class MiUser : IMiUser
	{
		private readonly HttpContext _context;
		private readonly UserModel _user;

		public MiUser(IHttpContextAccessor contextAccessor)
		{
			_context = contextAccessor.HttpContext!;
            _user = GetUser();
		}

		public long UserId => _user.UserId;

		public string UserName => _user.UserName;

		public bool IsSuperAdmin => _user.IsSuperAdmin;

		private UserModel GetUser()
		{
			return _context.Features.Get<UserModel>() ?? new UserModel();
		}
	}
}