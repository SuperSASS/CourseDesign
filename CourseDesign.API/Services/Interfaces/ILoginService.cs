using CourseDesign.Shared.DTOs;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 登录所需要的服务接口
    /// </summary>
    public interface ILoginService
    {
        Task<APIResponse> LoginAsync(string account, string password); // 登录
        Task<APIResponse> RegisterAsync(UserDTO user); // 注册
    }
}
