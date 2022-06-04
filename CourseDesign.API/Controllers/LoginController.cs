using CourseDesign.API.Services;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
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
        private readonly ILoginService service;
        public LoginController(ILoginService service) { this.service = service; }

        [HttpGet]
        public async Task<APIResponse> Login(string accuount, string password) => await service.LoginAsync(accuount, password);
        [HttpPost]
        public async Task<APIResponse> Register([FromBody] UserDTO model) => await service.Register(model);
    }
}
