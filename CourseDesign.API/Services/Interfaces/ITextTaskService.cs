using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 文本类计划<see cref="TextTaskDTO"/>类型的服务接口
    /// </summary>
    public interface ITextTaskService
    {
        Task<APIResponse> AddAsync(TextTaskDTO dbEntity); // 增
        Task<APIResponse> DeleteAsync(int id); // 删
        Task<APIResponse> GetAllAsync(QueryParameter parameter); // 按条件查询
        Task<APIResponse> UpdateAsync(TextTaskDTO dbUpdateEntity); // 改
    }
}