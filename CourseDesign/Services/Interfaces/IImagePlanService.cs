using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Command.Classes;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface IImagePlanService : IBaseService<ImagePlanDTO>
    {
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>
        public Task<APIResponse> Add(int user_id, ImagePlanClass imagePlan);
        public Task<APIResponse> Delete(int id);
        /// <summary>
        /// 返回该用户符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        public Task<APIResponse<PagedList<ImagePlanDTO>>> GetAllForUser(int user_id); 
        public Task<APIResponse> Update(ImagePlanClass imagePlan); 
    }
}
