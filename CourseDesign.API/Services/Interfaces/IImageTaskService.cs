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
        Task<APIResponse> AddAsync(ImagePlanDTO dbEntity); // 增
        Task<APIResponse> DeleteAsync(int id); // 删
        Task<APIResponse> GetAllForUserAsync(int user_id); // 查询某用户所有的图片类计划
        Task<APIResponse> UpdateAsync(ImagePlanDTO dbUpdateEntity); // 改
    }
}
