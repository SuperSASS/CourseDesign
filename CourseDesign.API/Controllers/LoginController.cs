using CourseDesign.API.Services;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// 用户登录的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService Service;
        public LoginController(ILoginService service) { Service = service; }

        /// <summary>
        /// 用户登录的实现
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpGet]
        public async Task<APIResponse> Login(string account, string password) => await Service.LoginAsync(account, password);

        /// <summary>
        /// 用户注册的实现
        /// </summary>
        /// <param name="dtoEntity">注册实体：账号、用户名、密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpPost]
        public async Task<APIResponse> Register([FromBody] UserDTO dtoEntity) => await Service.RegisterAsync(dtoEntity);
    }
}
