using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 战术人形相关的服务接口
    /// </summary>
    public interface ITDollService
    {
        Task<APIResponse> GetAllAsync(QueryParameter parameter); // 按条件查询人形
        Task<APIResponse> GetSingalAsync(int id); // 按ID查询人形
    }
}
