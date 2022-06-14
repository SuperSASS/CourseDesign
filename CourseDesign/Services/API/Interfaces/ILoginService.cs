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
        /// <see cref="APIResponse{UserDTO}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 操作成功后返回增加的结果，类型为<see cref="UserDTO"/>（可以用来加载用户上下文）</item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<UserDTO>> Login(UserDTO entity);

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

        /// <summary>
        /// [POST]更改用户信息
        /// </summary>
        /// <param name="entity"><see cref="UserDTO"/>类型，代表所修改的用户信息。（ID必须要有为UserID/Account恒null/若只修改用户名，密码需为null；修改密码反之）</param>
        /// <returns>
        /// <see cref="APIResponse{UserDTO}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 修改后的用户信息，类型为<see cref="UserDTO"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<UserDTO>> ChangeUserInfo(UserDTO entity);
    }
}
