using Mi.Core.Factory;
using Mi.Core.Toolkit.Helper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Controllers.BASE
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private readonly CaptchaFactory _captchaFactory;

        public PublicController(CaptchaFactory captchaFactory)
        {
            _captchaFactory = captchaFactory;
        }

        [HttpGet("captcha")]
        public FileResult LoginCaptcha()
        {
            var id = StringHelper.GetMacAddress();
            return File(_captchaFactory.NewByte(id),"image/png");
        }
    }
}