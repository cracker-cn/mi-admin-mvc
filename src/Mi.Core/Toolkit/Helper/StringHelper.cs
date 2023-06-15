﻿using System.Net.NetworkInformation;
using System.Security.Cryptography;

using Mi.Core.Service;

using Microsoft.Extensions.Options;

namespace Mi.Core.Toolkit.Helper
{
    public class StringHelper
    {
        public static string GetRandomString(int len)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new(GetNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (!reValue.Contains(s1, StringComparison.CurrentCulture)) reValue += s1;
            }
            return reValue;
        }

        public static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        public static string GetMacAddress()
        {
            try
            {
                NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface network in networks)
                {
                    if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        return network.GetPhysicalAddress().ToString();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Ouch(ex.Message);
            }
        }

        public static long ToLong(string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;
            long.TryParse(str, out long result);
            return result;
        }

        public static string UserKey(string userName, string roleName) => $"{userName}_function_{roleName}";

        public static string UserDataString(long id, string userName, string roleName) => $"{id}_{userName}_{roleName.Replace(',', '&')}";

        public static (long, string, string) GetUserData(string str)
        {
            var strArray = str.Split('_');
            return (long.Parse(strArray[0]), strArray[1], strArray[2]);
        }

        public static string UserCachePattern() => "\\.*(_function_){1}\\.*";

        public static string DefaultAvatar()
        {
            var webRootPath = DotNetService.Get<IOptionsMonitor<EnvironmentHandler>>().CurrentValue.WebRootPath;
            var path = Path.Combine(webRootPath, "admin", "images", "avatar.jpg");
            return FileHelper.GetBase64(path);
        }
    }
}
