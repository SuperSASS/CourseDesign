using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
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
        private readonly BasicSQLService<ImagePlan> imageDB;
        private readonly IMapper mapper;
        public ImagePlanService(IUnitOfWork unitOfWork, IMapper _mapper) { imageDB = new BasicSQLService<ImagePlan>(unitOfWork); mapper = _mapper; }

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="ImagePlan"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> AddAsync(ImagePlanDTO dtoEntity) { return await imageDB.AddAsync(mapper.Map<ImagePlan>(dtoEntity)); }// 通过AutoMapper，将DTO类型转化为数据库实体类

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> DeleteAsync(int id) { return await imageDB.DeleteAsync(id); }

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，查询用户ID为user_id"的所有打捞计划，按创建时间降序排序。
        /// <para>查询前100个返回。</para>
        /// </summary>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetAllForUserAsync(int user_id)
        {
            Expression<Func<ImagePlan, bool>> exp = (x) => x.UserID.Equals(user_id); // 查询满足搜索条件的元组
            return await imageDB.GetExpressionAllAsync(exp);
        }

        /// <summary>
        /// 在<see cref="ImagePlan"/>表中，修改元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> UpdateAsync(ImagePlanDTO dtoEntity) { return await imageDB.UpdateAsync(mapper.Map<ImagePlan>(dtoEntity)); }
    }
}
