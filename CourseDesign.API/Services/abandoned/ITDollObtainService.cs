using CourseDesign.API.Context;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.abandoned
{
    /// <summary>
    /// 用户所拥有战术人形的服务接口
    /// 【目前已被废弃，已经整合到了TDollService中……
    /// </summary>
    public interface ITDollObtainService
    {
        /// <summary>
        /// 用户获得新战术人形（来自于任务列表完成）
        /// </summary>
        /// <param name="dtoEntity">DTO层传过来的DTO实体</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> AddUserObtainTDollAsync(TDollObtainDTO dtoEntity);

        // 一般来说不存在删除

        /// <summary>
        /// 查找用户ID为user_ID所拥有的所有战术人形
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetUserAllObtainTDollAsync(int user_id);

        /// <summary>
        /// 将用户ID的所拥有战术人形，按照parameter条件查询，并分页展示。
        /// <para>条件为：单字段（除了status）、包含；status代表user_id是否拥有（0未拥有；1已拥有；null全部）</para>
        /// </summary>
        /// <param name="parameter">传来的<see cref="APIResponseInner"/>类型参数（若匹配<see cref="TextPlan.Status"/>，Search需要用<c>true/false</c>）</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetUserParamContainObtainTDollAsync(GETParameter parameter);

        /// <summary>
        /// 将用户ID的所拥有战术人形，按照parameter条件查询，并分页展示。
        /// <para>条件为：单字段（除了status）、匹配；status代表user_id是否拥有（0未拥有；1已拥有；null全部）</para>
        /// </summary>
        /// <param name="parameter">传来的<see cref="APIResponseInner"/>类型参数（若匹配<see cref="TextPlan.Status"/>，Search需要用<c>true/false</c>）</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponseInner"/></returns>
        Task<APIResponseInner> GetUserParamEqualObtainTDollAsync(GETParameter parameter);
    }
}
