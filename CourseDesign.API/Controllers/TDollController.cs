using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// 战术人形的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TDollController : ControllerBase
    {
        private readonly ITDollService Service;
        public TDollController(ITDollService service) { Service = service; }


        /// <summary>
        /// 用户获得新战术人形（目前只来自于任务列表完成）
        /// </summary>
        /// <param name="dtoEntity">DTO层传过来的DTO实体，分别含userId和tDollId</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回成功增加的的的单个元组（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        [HttpPost]
        public async Task<APIResponseInner> AddUserObtain([FromBody] TDollObtainDTO dtoEntity) => await Service.AddUserObtainTDollAsync(dtoEntity);

        /// <summary>
        /// 得到某一ID的战术人形元组。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的单个元组<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetID([FromQuery] int id) => await Service.GetIDAsync(id);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的战术人形元组。
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
        public async Task<APIResponseInner> GetUserAndParamContain([FromQuery] GETParameter parameter) => await Service.GetUserAndParamContainAsync(parameter);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的战术人形元组。
        /// <para>条件为：单字段匹配</para>
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到满足条件的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetUserAndParamEqual([FromQuery] GETParameter parameter) => await Service.GetUserAndParamEqualAsync(parameter);
    }
}
