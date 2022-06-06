using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.API.Services.Response;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// ImagePlan（计划列表的文字类计划）接口的实现
    /// </summary>
    public class ImagePlanService : IImagePlanService
    {
        private readonly BaseDBService<ImagePlan> imageDB; // 数据库服务器
        private readonly IMapper mapper;                     // 映射器

        // 构造函数，由注册依赖自己完成
        public ImagePlanService(IUnitOfWork unitOfWork, IMapper _mapper) { imageDB = new BaseDBService<ImagePlan>(unitOfWork); mapper = _mapper; }

        // 增
        public async Task<APIResponseInner> AddAsync(ImagePlanDTO dtoEntity) { return await imageDB.AddAsync(mapper.Map<ImagePlan>(dtoEntity)); } // 通过AutoMapper，将DTO类型转化为数据库实体类

        // 删
        public async Task<APIResponseInner> DeleteAsync(int id) { return await imageDB.DeleteAsync(id); }
        
        // 查ID
        public async Task<APIResponseInner> GetIDAsync(int id) { return await imageDB.GetIDAsync(id); }

        // 查用户所有
        public async Task<APIResponseInner> GetAllForUserAsync(int user_id)
        {
            Expression<Func<ImagePlan, bool>> exp = (x) => x.UserID == user_id; // 表达式
            return await imageDB.GetExpressionAllPagedAsync(exp);
        }

        // 改
        public async Task<APIResponseInner> UpdateAsync(ImagePlanDTO dtoEntity) { return await imageDB.UpdateAsync(mapper.Map<ImagePlan>(dtoEntity)); }
    }
}
