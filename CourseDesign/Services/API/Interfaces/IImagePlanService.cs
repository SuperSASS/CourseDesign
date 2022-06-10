using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Common.Classes;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.Interfaces
{
    public interface IImagePlanService : IBaseService<ImagePlanDTO>
    {
        #region 继承的基本方法
        //public new Task<APIResponse<ImagePlanDTO>> Add(ImagePlanDTO imagePlanDTO);
        //public new Task<APIResponse> Delete(int id);
        //public new Task<APIResponse<ImagePlanDTO>> GetID(int id);
        //public new Task<APIResponse<ImagePlanDTO>> Update(ImagePlanDTO imagePlanDTO);
        #endregion

        #region 额外的方法
        /// <summary>
        /// [GET]返回该用户所有结果。
        /// </summary>
        /// <param name="user_id">用户的ID</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 查询的分页结果集，类型为<see cref="PagedList{ImagePlanDTO}"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<PagedList<ImagePlanDTO>>> GetAllForUser(int user_id);
        #endregion
    }
}
