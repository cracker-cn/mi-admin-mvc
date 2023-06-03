using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mi.Core.Toolkit.Helper
{
    public class StringHelper
    {
        public static string GetRandomString(int len)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(getNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }

        public static int getNewSeed()
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
            catch(Exception ex)
            {
                throw new Ouch(ex.Message);
            }
        }
    }
}
