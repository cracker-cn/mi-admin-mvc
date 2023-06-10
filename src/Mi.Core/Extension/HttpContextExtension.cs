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
            var userData = context.User.FindFirst(ClaimTypes.UserData)?.Value;
            if (userData == null) return new UserModel();
            return JsonConvert.DeserializeObject<UserModel>(userData) ?? new UserModel();
        }
    }
}
