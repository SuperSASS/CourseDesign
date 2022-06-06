using CourseDesign.API.Context;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 图片类计划<see cref="ImagePlanDTO"/>的服务接口
    /// </summary>
    public interface IImagePlanService
    {
        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="ImagePlan"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> AddAsync(ImagePlanDTO dbEntity);

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> DeleteAsync(int id);

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，查询用户ID为user_id"的所有打捞计划，按创建时间降序排序。
        /// <para>查询前100个返回。</para>
        /// </summary>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetAllForUserAsync(int user_id);

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，修改元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> UpdateAsync(ImagePlanDTO dbUpdateEntity);
    }
}
