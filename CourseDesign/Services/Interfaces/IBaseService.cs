using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    /// <summary>
    /// 这个是作为一个基类来继承，可以避免其他类重复写Add、Delete和Update操作。
    /// </summary>
    /// <typeparam name="APPEntity">所继承的其他类型的服务（需要为DTO的，如<seealso cref="ImagePlanDTO"></seealso>）</typeparam>
    public interface IBaseService<APPEntity> where APPEntity : class
    {
        /// <summary>
        /// [POST]异步上传，上传数据到某一用户的记录中
        /// </summary>
        /// <param name="user_id">上传到哪个用户ID上</param>
        /// <param name="entity">上传的<see cref="APPEntity"/>类型实体</param>
        /// <returns> API返回的<see cref="APIResponse{APPEntity}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 上传成功后的数据，类型为<see cref="APPEntity"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        Task<APIResponse<APPEntity>> Add(int user_id, APPEntity entity);

        /// <summary>
        /// [DELETE]异步删除，删除唯一表示为ID的数据
        /// </summary>
        /// <param name="id">所删除数据的唯一标识ID</param>
        /// <returns> 
        /// <list type="bullet">
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        Task<APIResponse> Delete(int id);

        /// <summary>
        /// [POST]异步更新，更新用户的数据
        /// </summary>
        /// <param name="user_id">更新哪个用户ID上的数据</param>
        /// <param name="entity">更新的<see cref="APPEntity"/>类型实体</param>
        /// <returns> API返回的<see cref="APIResponse{APPEntity}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 更新成功后的数据，类型为<see cref="APPEntity"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        Task<APIResponse<APPEntity>> Update(int user_id, APPEntity entity);
    }
}