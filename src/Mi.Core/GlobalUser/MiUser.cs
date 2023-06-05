using System.Security.Claims;

using Mi.Core.Models;

using Microsoft.AspNetCore.Http;

namespace Mi.Core.GlobalUser
{
	public class MiUser : IMiUser
	{
		private readonly HttpContext _context;

		public MiUser(IHttpContextAccessor contextAccessor)
		{
			_context = contextAccessor.HttpContext!;
			LoadFetures();
		}

		public long UserId
		{
			get
			{
				if (long.TryParse(Get(ClaimTypes.NameIdentifier), out var id))
				{
					return id;
				}

				return -1;
			}
		}

		public string UserName
		{
			get
			{
				return Get(ClaimTypes.Name) ?? "";
			}
		}

		public bool IsSuperAdmin { get; set; } = false;

		private string? Get(string name)
		{
			return _context.User.Claims.FirstOrDefault(x => x.Type == name)?.Value;
		}

		private void LoadFetures()
		{
			var model = _context.Features.Get<UserModel>() ?? new UserModel();
			IsSuperAdmin = model.IsSuperAdmin;
		}
	}
}