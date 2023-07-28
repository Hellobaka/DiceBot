using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.Dice.Sdk.Cqp.Model;

namespace me.cqp.luohuaming.Dice.PublicInfos
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        public static string GetAppImageDirectory()
        {
            var ImageDirectory = Path.Combine(Environment.CurrentDirectory, "data", "image\\");
            return ImageDirectory;
        }

        public static string ComputeMD5(string input)
        {
            using(MD5 md5 = MD5.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(md5.ComputeHash(buffer, 0, buffer.Length));
            }
        }
    }
}
