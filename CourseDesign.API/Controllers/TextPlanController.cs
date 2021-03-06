using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// TextPlan（计划列表的文字类计划）的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TextPlanController : ControllerBase
    {
        private readonly ITextPlanService Service;
        public TextPlanController(ITextPlanService service) { Service = service; }

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="TextPlanDTO"/>类型元组</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所新增的单个元组</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpPost]
        public async Task<APIResponseInner> Add([FromBody] TextPlanDTO dtoEntity) => await Service.AddAsync(dtoEntity);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：只返回状态码为Success</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpDelete]
        public async Task<APIResponseInner> Delete(int id) => await Service.DeleteAsync(id);

        /// <summary>
        /// 对<see cref="TextPlanDTO"/>表进行“ID查询”操作。
        /// </summary>
        /// <param name="id">要查询元组的唯一标识ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的单个元组<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetID(int id) => await Service.GetIDAsync(id);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，获取用户ID为"user_id"的所有文字计划。
        /// </summary>
        /// <param name="user_id">传来的<see cref="APIResponseInner"/>用户ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetAllForUser([FromQuery] int user_id) => await Service.GetAllForUserAsync(user_id);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，获取用户ID为"user_id"，且满足parameter条件的所有元组，并分页展示。
        /// <para>条件为：单字段包含</para>
        /// </summary>
        /// <param name="param">传来的<see cref="APIResponseInner"/>类型参数</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到满足条件的所有元组集<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<APIResponseInner> GetParamContainForUser([FromQuery] GETParameter param) => await Service.GetParamForUserAsync(param); // 这里将user_id整合进了参数param里，因为分开两个参数第二个参数接收不到，暂且不知道为什么

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，修改元组"dtoEntity"。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所修改的单个元组</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        [HttpPost]
        public async Task<APIResponseInner> Update([FromBody] TextPlanDTO dtoEntity) => await Service.UpdateAsync(dtoEntity);

    }
}
