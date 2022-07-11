using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Tools
{
    public class MD5Tool
    {
        public static string GetMD5(string str)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] outputBytes = md5.ComputeHash(strBytes);
            return BitConverter.ToString(outputBytes).Replace("-", "");
        }
    }
}
