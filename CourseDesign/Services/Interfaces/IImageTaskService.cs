using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface IImagePlanService : IBaseService<ImagePlanDTO>
    {
        /// <summary>
        /// 返回该用户符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>
        public Task<APIResponse<PagedList<ImagePlanDTO>>> GetAllForUser(QueryParameter parameter); // TODO: 2 - 还没解决用户问题
    }
}
