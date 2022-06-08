using CourseDesign.API.Context;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 文本类计划<see cref="TextPlanDTO"/>类型的服务接口
    /// </summary>
    public interface ITextPlanService
    {
        /// <summary>
        /// 在<see cref="TextPlan"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="TextPlan"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> AddAsync(TextPlanDTO dbEntity);

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> DeleteAsync(int id);

        /// <summary>
        /// 对<see cref="TextPlan"/>表进行“ID查询”操作。
        /// </summary>
        /// <param name="id">要查询元组的唯一标识ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的单个元组<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetIDAsync(int id);

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，获取用户ID为"user_id"的所有文字计划。【这个功能其实在GetParamForUserAsync也有实现，可以考虑为容错
        /// </summary>
        /// <param name="user_id">传来的<see cref="APIResponseInner"/>用户ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetAllForUserAsync(int user_id);

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，获取用户ID为"user_id"，且满足parameter条件的所有元组，并分页展示。
        /// <para>条件为：单字段包含；这里的Status代表是否完成（0代表未完成）</para>
        /// </summary>
        /// <param name="parameter">传来的<see cref="APIResponseInner"/>类型参数</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetParamForUserAsync(GETParameter parameter);

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，修改元组"dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> UpdateAsync(TextPlanDTO dbUpdateEntity);
    }
}