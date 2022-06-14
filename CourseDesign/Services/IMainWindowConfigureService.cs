using CourseDesign.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 对于MainWindowViewModel，用来继承的接口，以让MainWindow进行登陆后配置
    /// </summary>
    /// 主要是因为MainWindowViewModel没法OnNavigatedTo，所以没法及时更新上下文
    public interface IMainWindowConfigureService
    {
        /// <summary>
        /// 对MainWindow进行登陆后配置
        /// </summary>
        /// <param name="user">登陆后的<see cref="UserClass"/>用户信息</param>
        void ConfigureForUser(UserClass user);

        /// <summary>
        /// 对MainWindow进行用户数据更新
        /// </summary>
        /// <param name="user">登陆后的<see cref="UserClass"/>用户信息</param>
        void UpdateForUser(UserClass user);
    }
}
