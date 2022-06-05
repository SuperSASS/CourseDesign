using CourseDesign.API.Context;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
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
        //Task<APIResponse> GetContainAsync(QueryParameter parameter); // 查 - 条件 - 单字段包含【无法在基类中实现，下放到具体子类中实现
        //Task<APIResponse> GetEqualAsync(QueryParameter parameter); // 查 - 条件 - 单字段匹配【无法在基类中实现，下放到具体子类中实现
        Task<APIResponse> GetAllAsync(); // 查 - 全查询
        Task<APIResponse> GetIDAsync(int id); // 查 - ID
        Task<APIResponse> GetExpressionSingalAsync(Expression<Func<DBEntity, bool>> exp); // 查 - 表达式 - 单值查询
        Task<APIResponse> GetExpressionAllAsync(Expression<Func<DBEntity, bool>> exp, int index = 0, int size = 100, Func<IQueryable<DBEntity>,IOrderedQueryable<DBEntity>> orderBy = null); // 查 - 表达式 - 分页查询
        Task<APIResponse> UpdateAsync(DBEntity dbUpdateEntity); // 改
    }
}
