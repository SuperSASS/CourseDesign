using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 对数据库各种实体的最基本服务（增删查改）
    /// </summary>
    public interface IBaseSQLService<T>
    {
        Task<APIResponse> GetAllAsync(QueryParameter parameter); // 查 - 条件
        Task<APIResponse> GetSingleAsync(int id); // 查 - 单值
        Task<APIResponse> AddAsync(T entity); // 增
        Task<APIResponse> UpdateAsync(T entity); // 查
        Task<APIResponse> DeleteAsync(int id); // 删
    }
}
