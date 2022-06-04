using CourseDesign.API.Context;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 图片类计划<see cref="ImageTaskDTO"/>的服务接口
    /// </summary>
    public interface IImageTaskService
    {
        Task<APIResponse> AddAsync(ImageTaskDTO dbEntity); // 增
        Task<APIResponse> DeleteAsync(int id); // 删
        Task<APIResponse> GetInTaskTDoll(int id); // 查询某战术人形是否在计划中
        Task<APIResponse> UpdateAsync(ImageTaskDTO dbUpdateEntity); // 改
    }
}
