using CourseDesign.API.Services;
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
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpPost]
        public async Task<APIResponse> Add([FromBody] TextPlanDTO dtoEntity) => await Service.AddAsync(dtoEntity);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpDelete]
        public async Task<APIResponse> Delete(int id) => await Service.DeleteAsync(id);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，获取用户ID为"user_id"的所有文字计划。
        /// </summary>
        /// <param name="user_id">传来的<see cref="APIResponse"/>用户ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpGet]
        public async Task<APIResponse> GetAllForUser([FromQuery] int user_id) => await Service.GetAllForUserAsync(user_id);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，获取用户ID为"user_id"，且满足parameter条件的所有元组，并分页展示。
        /// <para>条件为：单字段包含</para>
        /// </summary>
        /// <param name="param">传来的<see cref="APIResponse"/>类型参数</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpGet]
        public async Task<APIResponse> GetParamContainForUser([FromQuery] QueryParameter param) => await Service.GetParamForUserAsync(param);

        /// <summary>
        /// 在<see cref="TextPlanDTO"/>表中，修改元组"dtoEntity"。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        [HttpPost]
        public async Task<APIResponse> Update([FromBody] TextPlanDTO dtoEntity) => await Service.UpdateAsync(dtoEntity);

    }
}
