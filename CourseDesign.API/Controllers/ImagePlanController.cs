using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseDesign.API.Controllers
{
    /// <summary>
    /// ImagePlan（计划列表的文字类计划）的控制器层
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImagePlanController : ControllerBase
    {
        private readonly IImagePlanService Service;
        public ImagePlanController(IImagePlanService service) { Service = service; }

        /// <summary>
        /// 在<see cref="ImagePlanDTO"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="ImagePlanDTO"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> Add([FromBody] ImagePlanDTO dtoEntity) => await Service.AddAsync(dtoEntity);

        /// <summary>
        /// 在<see cref="ImagePlanDTO"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpDelete]
        public async Task<APIResponseInner> Delete(int id) => await Service.DeleteAsync(id);

        /// <summary>
        /// 对<see cref="ImagePlanDTO"/>表进行“ID查询”操作。
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
        /// 在<see cref="ImagePlanDTO"/>表中，查询用户ID为"user_id"的所有打捞计划。
        /// </summary>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpGet]
        public async Task<APIResponseInner> GetAllForUser(int user_id) => await Service.GetAllForUserAsync(user_id);

        /// <summary>
        /// 在<see cref="ImagePlanDTO"/>表中，修改元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        [HttpPost]
        public async Task<APIResponseInner> Update([FromBody] ImagePlanDTO dtoEntity) => await Service.UpdateAsync(dtoEntity);
    }
}
