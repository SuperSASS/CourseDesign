using CourseDesign.API.Services.abandoned;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers.abandoned
{
    /* 该控制器已废弃，合并到了TDollController中
    /// <summary>
    /// 用户所拥有战术人形的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TDollObtainController : ControllerBase
    {
        private readonly ITDollObtainService Service;
        public TDollObtainController(ITDollObtainService service) { Service = service; }

        /// <summary>
        /// [POST]用户获得新的战术人形。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="TextPlanDTO"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> Add([FromBody] TDollObtainDTO dtoEntity) => await Service.AddUserObtainTDollAsync(dtoEntity);


        /// <summary>
        /// [POST]得到某用户拥有的所有战术人形元祖。
        /// <para>条件为：单字段包含</para>
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetAll([FromQuery] int user_id) => await Service.GetUserAllObtainTDollAsync(user_id);

        /// <summary>
        /// [POST]得到某用户拥有的，满足<see cref="GETParameter"/>条件的战术人形元祖。
        /// <para>条件为：单字段包含</para>
        /// </summary>
        /// <param name="parameter">所需满足的条件（参数）</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到满足条件的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
		[HttpGet]
		public async Task<APIResponseInner> GetParamContain([FromQuery] GETParameter parameter) => await Service.GetUserParamContainObtainTDollAsync(parameter);

        /// <summary>
        /// 得到某用户拥有的，满足<see cref="GETParameter"/>条件的战术人形元组。
        /// <para>条件为：单字段匹配</para>
        /// </summary>
        /// <param name="parameter">所需满足的条件（参数）</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到满足条件的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetParamEqual([FromQuery] GETParameter parameter) => await Service.GetUserParamEqualObtainTDollAsync(parameter);
    }
    */
}
