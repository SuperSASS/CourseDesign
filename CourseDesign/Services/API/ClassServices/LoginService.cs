using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.API.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.ClassServices
{
    public class LoginService : ILoginService
    {
        public readonly HttpRestClient Client; // 向API发送请求的客户端，这里可认为只有RestClient这一个
        public readonly string ServiceName = "Login";    // 子服务名称

        public LoginService(HttpRestClient client)
        {
            Client = client;
        }

        // 登录，要返回登陆的用户UserDTO，以加载该用户数据
        public async Task<APIResponse<UserDTO>> Login(UserDTO entity)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/Login",
                Parameter = entity
            };
            return await Client.ExecuteAsync<UserDTO>(request);
        }

        // 注册
        public async Task<APIResponse> Register(UserDTO entity)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/Register",
                Parameter = entity
            };
            return await Client.ExecuteAsync(request);
        }

        // 更改用户信息
        public async Task<APIResponse<UserDTO>> ChangeUserInfo(UserDTO entity)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/ChangeUserInfo",
                Parameter = entity
            };
            return await Client.ExecuteAsync<UserDTO>(request);
        }

        // 验证用户密码
        public async Task<APIResponse> CheckPassword(UserDTO entity)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/CheckPassword",
                Parameter = entity
            };
            return await Client.ExecuteAsync(request);
        }
    }
}
