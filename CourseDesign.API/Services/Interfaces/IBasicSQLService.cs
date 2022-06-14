using CourseDesign.API.Constants;
using CourseDesign.API.Context;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 对数据库各种实体的最基本服务（增删查改），用以实现其他各服务的基本操作。
    /// </summary>
    /// 但注意这个不能像APP里的基本服务那样直接继承实现，因为这里还要将DTOEntity先Mappper为DBEntity
    internal interface IBasicSQLService<DBEntity> where DBEntity : BaseEntity
    {
        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“增添”操作。
        /// </summary>
        /// <param name="dbEntity">所要增添的<see cref="DBEntity"/>类型元组</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回成功增添的元组</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> AddAsync(DBEntity dbEntity);

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“删除”操作。
        /// </summary>
        /// <param name="id">要删除元组的唯一标识ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：只返回状态码Success</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> DeleteAsync(int id);

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“全查询”操作。
        /// </summary>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的元组集，为List类型<para>（若未找到则Result为null）</para><para>【⚠小心JSON在转换的时候用的是PagedList从而出错！</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetAllAsync(); // 查 - 全查询

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“ID查询”操作。
        /// </summary>
        /// <param name="id">要查询元组的唯一标识ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的单个元组<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetIDAsync(int id);

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“表达式、单条查询”操作。（就是个扩展接口，需要自己写表达式）
        /// </summary>
        /// <param name="exp">要查询的表达式，类型为<see cref="Expression{Func{DBEntity, bool}}"/></param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的单个元组<para></para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetExpressionSingalAsync(Expression<Func<DBEntity, bool>> exp);

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“表达式、多条、分页查询”操作。（就是个扩展接口，需要自己写表达式）
        /// </summary>
        /// <param name="exp">要查询的表达式，类型为<see cref="Expression{Func{DBEntity, bool}}"/></param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回所查询到的元组集，为PagedList类型<para>（若未找到则Result属性为null）</para></item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetExpressionAllPagedAsync(Expression<Func<DBEntity, bool>> exp, int index, int size, Func<IQueryable<DBEntity>, IOrderedQueryable<DBEntity>> orderBy = null); // 查 - 表达式 - 分页查询

        /// <summary>
        /// 对<see cref="DBEntity"/>表进行“修改”操作。
        /// </summary>
        /// <param name="dbUpdateEntity">需要修改的<see cref="DBEntity"/>类型元组</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并在Result返回成功修改的元组</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> UpdateAsync(DBEntity dbUpdateEntity); // 改

        #region 废弃、待实现接口
        //Task<APIResponse> GetContainAsync(QueryParameter parameter); // 查 - 条件 - 单字段包含【无法在基类中实现，下放到具体子类中实现
        //Task<APIResponse> GetEqualAsync(QueryParameter parameter); // 查 - 条件 - 单字段匹配【无法在基类中实现，下放到具体子类中实现
        #endregion
    }
}
