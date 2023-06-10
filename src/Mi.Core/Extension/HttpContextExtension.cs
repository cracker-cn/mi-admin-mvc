using System.Security.Claims;

using Mi.Core.Models;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Mi.Core.Extension
{
    public static class HttpContextExtension
    {
        public static UserModel GetUser(this HttpContext context)
        {
            return context.Features.Get<UserModel>() ?? new UserModel();
        }
    }
}
