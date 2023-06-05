using System.Security.Cryptography;
using System.Text;

namespace Mi.Core.Toolkit.Helper
{
    /// <summary>
    /// 加密
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// 获取根据盐码加密的密码
        /// </summary>
        /// <param name="password">原密码</param>
        /// <param name="salt">盐码</param>
        /// <returns></returns>
        public static string GenEncodingPassword(string password
            , string salt)
        {
            var md5 = MD5.Create();
            var bs = Encoding.UTF8.GetBytes(password + salt);
            var hs = md5.ComputeHash(bs);
            var stringBuilder = new StringBuilder();
            foreach (var item in hs) stringBuilder.Append(item.ToString("x2"));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取新的密码盐码
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordSalt()
        {
            var salt = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}