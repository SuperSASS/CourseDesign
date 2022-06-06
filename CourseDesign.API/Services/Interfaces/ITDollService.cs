using CourseDesign.API.Services.Response;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.API.Services.Interfaces
{
    /// <summary>
    /// 战术人形相关的服务接口
    /// </summary>
    public interface ITDollService
    {

        /// <summary>
        /// 得到某一ID的战术人形元组。
        /// </summary>
        /// <param name="id">唯一标识ID</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回查询的的单个元组（若未找到则Result属性为null）（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> GetIDAsync(int id); // 按ID查询人形

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的所有战术人形元组。（条件为：单字段、包含）
        /// <para>（注：Field参数为空，代表按分页全查询，否则参数查询）（若PageSize=0，默认为100）</para>
        /// </summary>
        /// <param name="parameter">参数，其中若指定user_id，是查找某一用户的人形，否则是在图鉴里查询全部人形</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回查询的的元组集，为PagedList类型<para>（若未找到则Result属性为null）（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> GetParamContainAsync(GETParameter parameter);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的所有战术人形元组。（条件为：单字段、匹配）
        /// <para>（注：Field参数为空，代表按分页全查询，否则参数查询）（若PageSize=0，默认为100）</para>
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回查询的的元组集，为PagedList类型<para>（若未找到则Result属性为null）（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> GetParamEqualAsync(GETParameter parameter); 
    }
}
