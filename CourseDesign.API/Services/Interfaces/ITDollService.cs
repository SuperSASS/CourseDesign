using CourseDesign.Shared.DTOs;
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
        /// 用户获得新战术人形（目前只来自于任务列表完成）
        /// </summary>
        /// <param name="dtoEntity">DTO层传过来的DTO实体，分别含userId和tDollId</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回成功增加的的的单个元组（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        Task<APIResponseInner> AddUserObtainTDollAsync(TDollObtainDTO dtoEntity);

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
        /// 得到某用户拥有的满足<see cref="GETParameter"/>条件的所有战术人形元组。（条件为：某用户、单字段、包含）（PageIndex默认为0，PageSize默认为100）
        /// <list type="bullet">
        /// <item>user_id参数为空，代表按图鉴全查询</item>
        /// <item>field参数为空，代表按分页全查询</item>
        /// </list>
        /// </summary>
        /// <param name="parameter">参数，其中若指定user_id，是查找某一用户的人形，否则是在图鉴里查询全部人形；这里的Status代表是否拥有，0是未拥有</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回查询的的元组集，为PagedList类型（若未找到则Result属性为null）（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetUserAndParamContainAsync(GETParameter parameter);

        /// <summary>
        /// 得到满足<see cref="GETParameter"/>条件的所有战术人形元组。（条件为：某用户、单字段、匹配）（PageIndex默认为0，PageSize默认为100）
        /// <list type="bullet">
        /// <item>user_id参数为空，代表按图鉴全查询</item>
        /// <item>field参数为空，代表按分页全查询</item>
        /// </list>
        /// </summary>
        /// <param name="parameter">参数，其中若指定user_id，是查找某一用户的人形，否则是在图鉴里查询全部人形；这里的Status代表是否拥有，0是未拥有</param>
        /// <returns>API返回消息<see cref="APIResponseInner"/>
        /// <list type="bullet">
        /// <item>成功：状态码为Success，并返回查询的的元组集，为PagedList类型（若未找到则Result属性为null）（</item>
        /// <item>失败：返回相应错误代码和信息</item>
        /// </list>
        /// </returns>
        Task<APIResponseInner> GetUserAndParamEqualAsync(GETParameter parameter); 
    }
}
