using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 战术人形相关的服务接口
    /// </summary>
    public interface ITDollService
    {
        Task<APIResponse> GetParamContainAsync(QueryParameter parameter); // 按条件包含查询人形
        Task<APIResponse> GetParamEqualAsync(QueryParameter parameter); // 按条件匹配查询人形
        Task<APIResponse> GetIDAsync(int id); // 按ID查询人形
    }
}
