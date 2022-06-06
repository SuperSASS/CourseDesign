using CourseDesign.Shared.DTOs;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 登录所需要的服务接口
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：只返回状态码为Success</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> LoginAsync(string account, string password);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">注册的实体，需要提供：账号Account、用户名UserName、密码Password</param>
        /// <list type="bullet">
        /// <item>成功：返回状态码为Success，以及在Result返回注册成功的实体</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> RegisterAsync(UserDTO user);
    }
}
