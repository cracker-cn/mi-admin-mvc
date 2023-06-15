using SimpleCaptcha;

namespace Mi.Core.Factory
{
    public class CaptchaFactory
    {
        private readonly ICaptcha _captcha;

        public CaptchaFactory(ICaptcha captcha)
        {
            _captcha = captcha;
        }

        public byte[] NewByte(string id)
        {
            var info = _captcha.Generate(id);
            using (var stream = new MemoryStream(info.CaptchaByteData))
            {
                var bt = new byte[stream.Length];
                stream.Read(bt, 0, bt.Length);
                return bt;
            }
        }

        public bool Validate(string id, string code)
        {
            return _captcha.Validate(id, code);
        }
    }
}