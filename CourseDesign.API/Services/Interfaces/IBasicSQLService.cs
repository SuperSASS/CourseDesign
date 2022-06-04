using CourseDesign.API.Context;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 对数据库各种实体的最基本服务（增删查改），用以实现其他各服务的基本操作
    /// </summary>
    public interface IBasicSQLService<DBEntity> where DBEntity : BaseEntity
    {
        Task<APIResponse> AddAsync(DBEntity dbEntity); // 增
        Task<APIResponse> DeleteAsync(int id); // 删
        Task<APIResponse> GetAllAsync(QueryParameter parameter); // 查 - 条件
        Task<APIResponse> GetSingleAsync(int id); // 查 - 单值
        Task<APIResponse> UpdateAsync(DBEntity dbUpdateEntity); // 改
    }
}
