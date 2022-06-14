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
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> Login([FromBody] UserDTO dtoEntity) => await Service.LoginAsync(dtoEntity);

        /// <summary>
        /// 用户注册的实现
        /// </summary>
        /// <param name="dtoEntity">注册实体：账号、用户名、密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> Register([FromBody] UserDTO dtoEntity) => await Service.RegisterAsync(dtoEntity);

        /// <summary>
        /// 更改用户信息
        /// </summary>
        /// <param name="dtoEntity">所更改的实体：ID、用户名或密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> ChangeUserInfo([FromBody] UserDTO dtoEntity) => await Service.ChangeUserInfoAsync(dtoEntity);

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="dtoEntity">所需验证的实体：ID、密码</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> CheckPassword([FromBody] UserDTO dtoEntity) => await Service.CheckPassword(dtoEntity);
    }
}
