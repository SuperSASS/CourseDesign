using CourseDesign.Shared.DTOs;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 登录所需要的服务
    /// </summary>
    public interface ILoginService
    {
        Task<APIResponse> LoginAsync(string account, string password);
        Task<APIResponse> Register(UserDTO user);
    }
}
