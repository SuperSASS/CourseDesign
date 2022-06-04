using Arch.EntityFrameworkCore.UnitOfWork;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// 最基础的SQL操作实现
    /// <para>对单个表（<see cref="DBEntity"/>类型）的增删查改操作（查分为单值查询和条件查询）</para>
    /// </summary>
    /// <typeparam name="DBEntity">操作的数据库实体类（注：一定注意这是对数据库的操作，给的是数据库类型，即Mapper在服务层映射）</typeparam>
    public class BasicSQLService<DBEntity> : IBasicSQLService<DBEntity> where DBEntity : BaseEntity // 注：这边where DBEntity : BaseEntity，约束传进来的泛型必须是数据库基本类型实体，后面ID就都可以用了，很强大！
    {
        private readonly IUnitOfWork UnitOfWork;     // 工作单元
        private readonly IRepository<DBEntity> Repo; // 仓储

        /// <summary>
        /// SQL操作的构造函数，由注册依赖自己构造，负责初始化工作单元和仓储
        /// </summary>
        public BasicSQLService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repo = unitOfWork.GetRepository<DBEntity>();
        }

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“增添”操作
        /// </summary>
        /// <param name="dbEntity">所要增添的<see cref="DBEntity"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> AddAsync(DBEntity dbEntity)
        {
            try
            {
                await Repo.InsertAsync(dbEntity);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 尝试保存插入操作
                    return new APIResponse(dbEntity);
                else
                    throw new Exception("数据添加失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“删除”操作
        /// </summary>
        /// <param name="id">要删除元组的<c>ID</c></param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> DeleteAsync(int id)
        {
            try
            {
                var task = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 先查元组
                // TODO: ⚠警告！！这里需要测试，删除没找到时，Delete是否有问题
                Repo.Delete(task);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 上传插入
                    return new APIResponse();
                else
                    throw new Exception("数据删除失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“条件查询”操作
        /// </summary>
        /// <param name="parameter">查询的条件</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                // TODO: 1! - 验证这样编写是否正确
                // TODO: 3 - 支持不同种类排序
                var tasks = await Repo.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true :
                    x.GetType().GetField(parameter.Field).GetValue(x).ToString().Contains(parameter.Search), // 查询满足搜索条件的元组
                    pageSize: parameter.PageSize,   // 每页最多个数
                    pageIndex: parameter.PageIndex, // PageSize下，第多少页
                    orderBy: source => source.OrderByDescending(t => t.CreateDate)); // 按创建时间降序排序
                return new APIResponse(tasks);
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“单值查询”操作
        /// </summary>
        /// <param name="id">要查询元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetSingleAsync(int id)
        {
            try
            {
                var task = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 查该ID元组
                return new APIResponse(task);
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“修改”操作
        /// </summary>
        /// <param name="dbUpdateEntity">需要修改的<see cref="DBEntity"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> UpdateAsync(DBEntity dbUpdateEntity)
        {
            try
            {
                var entity = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(dbUpdateEntity.ID)); // 先找到该元组
                if (entity == null)
                    throw new Exception("不存在这条数据哦_(:зゝ∠)_……");

                entity = dbUpdateEntity;
                Repo.Update(entity);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 尝试保存修改操作
                    return new APIResponse(entity);
                else
                    throw new Exception("数据修改失败啦qwq，肯定不是服务器的问题！……");
            }
            catch (Exception ex)
            {
                return new APIResponse(ex.Message);
            }
        }
    }
}
