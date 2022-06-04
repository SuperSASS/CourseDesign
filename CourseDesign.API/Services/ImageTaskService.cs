using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// ImageTask（计划列表的文字类计划）接口的实现
    /// </summary>
    public class ImageTaskService : IImageTaskService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<ImageTask> ImageTaskRepo;
        private readonly IMapper Mapper;
        public ImageTaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            ImageTaskRepo = UnitOfWork.GetRepository<ImageTask>();
            Mapper = mapper;
        }

        /// <summary>
        /// 在ImageTask表中，异步添加元组entity
        /// </summary>
        /// <param name="entity">要添加到元组</param>
        /// <returns>操作返回消息</returns>
        public async Task<APIResponse> AddAsync(ImageTaskDTO entity)
        {
            try
            {
                var dbEntity = Mapper.Map<ImageTask>(entity); // 通过AutoMapper，将DTO类型转化为数据库实体类
                await ImageTaskRepo.InsertAsync(dbEntity);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 上传插入
                    return new APIResponse(entity);
                return new APIResponse("数据添加失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 在ImageTask表中，异步删除ID为id的元组。
        /// <para>感觉有点问题这个实现</para>
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns></returns>
        public async Task<APIResponse> DeleteAsync(int id)
        { // TODO: 警告：听说有问题
            try
            {
                var task = await ImageTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 先查元组
                ImageTaskRepo.Delete(task);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 上传插入
                    return new APIResponse();
                return new APIResponse("数据删除失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 在ImageTask表中，获取所有满足条件的元组，并分页显示。【TODO：在ImageTask中应该不存在条件查询，不过暂时写上
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var tasks = await ImageTaskRepo.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.TDoll_ID.Equals(parameter.Search),
                    pageSize: parameter.PageSize,   // 每页最多个数
                    pageIndex: parameter.PageIndex, // PageSize下，第多少页
                    orderBy: source => source.OrderByDescending(t => t.CreateDate)); // 按条件查寻元组，按创建时间降序排序
                return new APIResponse(tasks);
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 在ImageTask表中，查找ID为id的元组。
        /// </summary>
        /// <param name="id">要查找的元组的ID</param>
        /// <returns></returns>
        public async Task<APIResponse> GetSingleAsync(int id)
        {
            try
            {
                var task = await ImageTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 查元组
                return new APIResponse(task);
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 在ImageTask表中，修改ID为entiyu.ID元组。
        /// </summary>
        /// <param name="entity">要修改成什么新元组</param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateAsync(ImageTaskDTO entity)
        {
            try
            {
                var dbEntity = Mapper.Map<ImageTask>(entity); // DTO映射db
                var task = await ImageTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(dbEntity.ID)); // 查元组
                if (task == null)
                    return new APIResponse("不存在这条数据哦_(:зゝ∠)_……");

                task.TDoll_ID = dbEntity.TDoll_ID;
                task.Status = dbEntity.Status;
                task.UpdateDate = DateTime.Now;
                ImageTaskRepo.Update(task);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 上传修改
                    return new APIResponse(task);
                return new APIResponse("数据修改失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
    }
}
