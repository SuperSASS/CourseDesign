using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<APIResponse<TEntity>> AddAsync(TEntity entity);
        Task<APIResponse<TEntity>> UpdateAsync(TEntity entity);
        Task<APIResponse> DeleteAsync(int id);
        Task<APIResponse<TEntity>> GetFirstDefaultAsync(int id);
        Task<APIResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter);
    }
}