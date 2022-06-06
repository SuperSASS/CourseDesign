using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Command.Classes;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface ITextPlanService : IBaseService<TextPlanDTO>
    {
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>
        public Task<APIResponse<TextPlanDTO>> AddAsync(TextPlanDTO imagePlan);
        public Task<APIResponse> DeleteAsync(int id);
        /// <summary>
        /// 返回该用户符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        public Task<APIResponse<PagedList<TextPlanDTO>>> GetAllForUserAsync(int user_id);
        public Task<APIResponse<PagedList<TextPlanDTO>>> GetParamContainForUserAsync(int user_id, QueryParameter parameter);
        public Task<APIResponse<TextPlanDTO>> UpdateAsync(TextPlanDTO imagePlan);
    }
}
