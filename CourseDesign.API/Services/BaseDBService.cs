using Arch.EntityFrameworkCore.UnitOfWork;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseDesign.API.Constants;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// 这里面实现了些最基础的数据库DB的SQL操作，因此其他各服务直接调用这些操作即可
    /// <para>即对单个表（<see cref="DBEntity"/>类型）的增、删、查、改、操作（查分为所有、ID、表达式查）</para>
    /// <para>数据基本属性<c>ID, CreateDate, UpdateDate</c>会在这里处理，因此外部只用确保额外属性的正确即可</para>
    /// </summary>
    /// <typeparam name="DBEntity">操作的数据库实体类（注：一定注意这是对数据库的操作，给的是数据库类型，即Mapper在Server中映射）</typeparam>
    public class BaseDBService<DBEntity> : IBasicSQLService<DBEntity> where DBEntity : BaseEntity // 注：这边where DBEntity : BaseEntity，约束传进来的泛型必须是数据库基本类型实体，后面ID就都可以用了，很强大！
    {
        private readonly IUnitOfWork UnitOfWork;     // 工作单元
        private readonly IRepository<DBEntity> Repo; // 仓储

        // SQL操作的构造函数，由注册依赖自己构造，负责初始化工作单元和仓储
        public BaseDBService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repo = unitOfWork.GetRepository<DBEntity>();
        }

        #region 方法
        // 增
        public async Task<APIResponseInner> AddAsync(DBEntity dbEntity)
        {
            try
            {
                dbEntity.ID = 0; // ID自增
                dbEntity.CreateDate = dbEntity.UpdateDate = DateTime.Now; // 将创建日期、修改日期设为服务器当前日期
                await Repo.InsertAsync(dbEntity);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 尝试保存插入操作，成功会返回添加的数据
                    return new APIResponseInner(dbEntity);
                else
                    return new APIResponseInner(APIStatusCode.Add_Failed, "数据添加失败啦qwq……"); // 感觉插入失败好像会直接抛异常，不会走到这来orz【下面的也一样……
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }

        // 删
        public async Task<APIResponseInner> DeleteAsync(int id)
        {
            try
            {
                var task = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id)); // 先查元组
                Repo.Delete(task);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 上传插入
                    return new APIResponseInner();
                else
                    return new APIResponseInner(APIStatusCode.Delete_Failed, "数据删除失败啦qwq……");
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + (ex.InnerException != null ? "\n" + ex.InnerException.Message : ""));
            }
        }

        // 查 - 所有
        public async Task<APIResponseInner> GetAllAsync()
        {
            try
            {
                var gets = await Repo.GetAllAsync();
                return new APIResponseInner(gets);
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }

        // 查 - ID查
        public async Task<APIResponseInner> GetIDAsync(int id)
        {
            try
            {
                var get = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(id));
                return new APIResponseInner(get);
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }

        // 查 - 表达式 - 单条查询
        public async Task<APIResponseInner> GetExpressionSingalAsync(Expression<Func<DBEntity, bool>> exp)
        {
            try
            {
                var get = await Repo.GetFirstOrDefaultAsync(predicate: exp);
                return new APIResponseInner(get);
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }

        // 查 - 表达式 - 多条查询
        public async Task<APIResponseInner> GetExpressionAllPagedAsync(Expression<Func<DBEntity, bool>> exp, int index = PageConst.PageIndex, int size = PageConst.PageSize, Func<IQueryable<DBEntity>, IOrderedQueryable<DBEntity>> order = null)
        {
            try
            {
                var gets = await Repo.GetPagedListAsync(predicate: exp,
                    pageIndex: index,
                    pageSize: size,
                    orderBy: order == null ? source => source.OrderByDescending(t => t.CreateDate) : order);
                return new APIResponseInner(gets);
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }

        // 改
        public async Task<APIResponseInner> UpdateAsync(DBEntity dbUpdateEntity)
        {
            try
            {
                var dbEntity = await Repo.GetFirstOrDefaultAsync(predicate: x => x.ID.Equals(dbUpdateEntity.ID)); // 先找到该元组
                if (dbEntity == null)
                    return new APIResponseInner(APIStatusCode.Update_Not_Haven, "不存在这条数据哦_(:зゝ∠)_……");

                BaseEntity bak = dbEntity;
                dbEntity = dbUpdateEntity;
                // ID和创建日期不变，这里要先备份再还原
                dbEntity.ID = bak.ID;
                dbEntity.CreateDate = bak.CreateDate;
                dbEntity.UpdateDate = DateTime.Now;
                Repo.Update(dbEntity);
                if (await UnitOfWork.SaveChangesAsync() > 0) // 尝试保存修改操作
                    return new APIResponseInner(dbEntity);
                else
                    return new APIResponseInner(APIStatusCode.Update_Failed, "数据修改失败啦qwq，肯定不是服务器的问题！……");
            }
            catch (Exception ex)
            {
                return new APIResponseInner(APIStatusCode.Unknown_Error, ex.Message + ex.InnerException != null ? "\n" + ex.InnerException.Message : "");
            }
        }
        #endregion 方法

        #region 废弃、未实现方法

        //public async Task<APIResponse> GetContainAsync(QueryParameter parameter)
        //{
        //    try
        //    {
        //        var gets = await Repo.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(parameter.Search) ? true : // Search参数为空，代表全条件查询
        //            x.GetType().GetField(parameter.Field).GetValue(x).ToString().Contains(parameter.Search), // 查询满足搜索条件的元组
        //            pageSize: parameter.PageSize,   // 每页最多个数
        //            pageIndex: parameter.PageIndex, // PageSize下，第多少页
        //            orderBy: source => source.OrderByDescending(t => t.CreateDate)); // 按创建时间降序排序
        //        return new APIResponse(gets);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse(ex.Message);
        //    }
        //}
        // 上述方法不正确，待之后实现……
        // 记录：应该要采用“反射、Expression表达式”

        //public async Task<APIResponse> GetEqualAsync(QueryParameter parameter)
        //{
        //    try
        //    {
        //        //Expression<Func<DBEntity, bool>> exp = (x) =>
        //        //    string.IsNullOrWhiteSpace(parameter.Search) ? true : // Search参数为空，代表全条件查询
        //        //    x.GetType().GetProperty(parameter.Field).GetValue(x).ToString().Equals(parameter.Search); // 查询满足搜索条件的元组
        //        Expression<Func<DBEntity, bool>> exp = x => (x.GetType().GetProperty("Account").GetValue(x).ToString().Equals("string"));
        //        var gets = await Repo.GetPagedListAsync(predicate: exp,
        //            pageSize: parameter.PageSize,   // 每页最多个数
        //            pageIndex: parameter.PageIndex, // PageSize下，第多少页
        //            orderBy: source => source.OrderByDescending(t => t.CreateDate)); // 按创建时间降序排序
        //        return new APIResponse(gets);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIResponse(ex.Message);
        //    }
        //}
        // 上述方法不正确，待之后实现……
        #endregion
    }
}