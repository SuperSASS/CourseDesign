using System.Collections.Generic;

namespace CourseDesign.Shared.DTOs
{
    public class UserDTO : BaseDTO
    {
        private string account;
        private string userName;
        private string password;
        private List<int> tdollObtain;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 拥有的战术人形集合
        /// </summary>
        public List<int> TDollObtain
        {
            get { return tdollObtain; }
            set { tdollObtain = value; OnPropertyChanged(); }
        }

        // TODO: 还有用户的一些自定义选项应该存储，特别是头像系统！
    }
}
