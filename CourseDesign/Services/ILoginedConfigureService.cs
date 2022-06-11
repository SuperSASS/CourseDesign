using CourseDesign.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 用户登录成功后后系统初始化配置服务（读取用户上下文）
    /// </summary>
    public interface ILoginedConfigureService
    {
        void ConfigureForUser(UserClass user);
    }
}
