using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.Interfaces
{
    public interface ILoginService 
    {
        /// <summary>
        /// [POST]用户登录
        /// </summary>
        /// <param name="entity"><see cref="UserDTO"/>类型，代表登录用户的数据，只用传递Account和Password。</param>
        /// <returns>
        /// <see cref="APIResponse"/>类型消息
        /// <list type="bullet">
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse> Login(UserDTO entity);

        /// <summary>
        /// [POST]用户注册
        /// </summary>
        /// <param name="entity"><see cref="UserDTO"/>类型，代表所注册的账户数据。</param>
        /// <returns>
        /// <see cref="APIResponse"/>类型消息
        /// <list type="bullet">
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse> Register(UserDTO entity);
    }
}
