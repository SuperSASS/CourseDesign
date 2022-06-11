using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Classes
{
    public class UserClass
    {
        private int userID;
        private string userName;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get { return userID; } set { userID = value; } }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get { return userName; } set { userName = value; } }
        // TODO: 3 - 用户个人配置：如头像、副官选择等

        public UserClass(int userID, string userName)
        {
            UserID = userID;
            UserName = userName;
        }
    }
}
