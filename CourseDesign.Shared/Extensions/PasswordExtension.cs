using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Shared.Extensions
{
    public static class PasswordExtension
    {
        /// <summary>
        /// 得到字符串密码数据的MD5，用于加密
        /// </summary>
        /// <param name="data">所给的密码</param>
        /// <returns>MD5处理后的密码</returns>
        /// <exception cref="ArgumentNullException">密码为空</exception>
        public static string GetMD5(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException(nameof(data));

            var hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(data));
            return Convert.ToBase64String(hash);//将加密后的字节数组转换为加密字符串
        }
    }
}
