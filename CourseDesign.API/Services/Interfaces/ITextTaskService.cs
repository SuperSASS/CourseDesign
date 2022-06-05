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
        Task<APIResponse> AddAsync(TextPlanDTO dbEntity); // 增
        Task<APIResponse> DeleteAsync(int id); // 删
        Task<APIResponse> GetAllForUserAsync(int user_id); // 查询某用户的所有文本计划
        Task<APIResponse> GetParamForUserAsync(int user_id, QueryParameter parameter); // 按条件查询某用户的文本计划
        Task<APIResponse> UpdateAsync(TextPlanDTO dbUpdateEntity); // 改
    }
}