using Mi.Core.Factory;
using Mi.Core.Models.UI;
using Mi.Core.Toolkit.Helper;
using Mi.IService.Public;

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
        private readonly IPublicService _publicService;

        public PublicController(CaptchaFactory captchaFactory, IPublicService publicService)
        {
            _captchaFactory = captchaFactory;
            _publicService = publicService;
        }

        [HttpGet("captcha")]
        public FileResult LoginCaptcha()
        {
            var id = StringHelper.GetMacAddress();
            return File(_captchaFactory.NewByte(id), "image/png");
        }

        [HttpGet("config")]
        public async Task<PaConfigModel> Config()
        {
            return await _publicService.ReadConfigAsync();
        }
    }
}