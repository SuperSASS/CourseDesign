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
    /// TextTask（计划列表的图片类计划）接口的实现
    /// </summary>
    public class TextTaskService : ITextTaskService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<TextTask> TextTaskRepo;
        private readonly IMapper Mapper;
        public TextTaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            TextTaskRepo = UnitOfWork.GetRepository<TextTask>();
            Mapper = mapper;
        }

        /// <summary>
        /// 在TextTask表中，异步添加元组entity
        /// </summary>
        /// <param name="entity">要添加到元组</param>
        /// <returns>操作返回消息</returns>
        public async Task<APIResponse> AddAsync(TextTaskDTO entity)
        {
            try
            {
                var dbEntity = Mapper.Map<TextTask>(entity); // 通过AutoMapper，将DTO类型转化为数据库实体类
                await TextTaskRepo.InsertAsync(dbEntity);
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
        /// 在TextTask表中，异步删除ID为id的元组。
        /// <para>感觉有点问题这个实现</para>
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns></returns>
        public async Task<APIResponse> DeleteAsync(int id)
        {
            try
            {
                var task = await TextTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 先查元组
                TextTaskRepo.Delete(task);
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
        /// 在TextTask表中，获取满足条件的所有元组，并分页展示。
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var tasks = await TextTaskRepo.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Equals(parameter.Search),
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
        /// 在TextTask表中，查找ID为id的元组。
        /// </summary>
        /// <param name="id">要查找的元组的ID</param>
        /// <returns></returns>
        public async Task<APIResponse> GetSingleAsync(int id)
        {
            try
            {
                var task = await TextTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 查元组
                return new APIResponse(task);
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }

        /// <summary>
        /// 在TextTask表中，修改ID为entiyu.ID元组。
        /// </summary>
        /// <param name="entity">要修改成什么新元组</param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateAsync(TextTaskDTO entity)
        {
            try
            {
                var dbEntity = Mapper.Map<TextTask>(entity); // DTO映射db
                var task = await TextTaskRepo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(dbEntity.ID)); // 查元组
                if (task == null)
                    return new APIResponse("不存在这条数据哦_(:зゝ∠)_……");

                task.Title = dbEntity.Title;
                task.Content = dbEntity.Content;
                task.Status = dbEntity.Status;
                task.UpdateDate = DateTime.Now;
                TextTaskRepo.Update(task);
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
