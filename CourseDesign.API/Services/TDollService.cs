using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CourseDesign.API.Services.APIResponse;

namespace CourseDesign.API.Services
{
    public class TDollService : ITDollService
    {
        private readonly BasicSQLService<TDoll> tDollDB;
        private readonly IMapper mapper;

        public TDollService(IUnitOfWork unitOfWork, IMapper _mapper) { tDollDB = new BasicSQLService<TDoll>(unitOfWork); mapper = _mapper; }


        /// <summary>
        /// 得到满足<see cref="QueryParameter"/>条件的所有战术人形元组。
        /// <para>条件为：单字段包含（注：Field参数为空，代表按分页全查询，否则参数查询）（若PageSize=0，默认为100）</para>
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetParamContainAsync(QueryParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            if (string.IsNullOrWhiteSpace(parameter.search))  // Search参数为空，代表按分页全条件查询
                return await tDollDB.GetExpressionAllAsync(exp = (x) => true, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
            else
            {
                switch (parameter.field)
                {
                    case "Name":
                        exp = (x) => x.Name.Contains(parameter.search);
                        break;
                    default:
                        return new APIResponse(StatusCode.Select__Wrong_filed, "该字段无法使用包含查询");
                }
                return await tDollDB.GetExpressionAllAsync(exp, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
            }
        }

        /// <summary>
        /// 得到满足<see cref="QueryParameter"/>条件的所有战术人形元组。
        /// <para>条件为：单字段匹配（注：Field参数为空，代表按分页全查询，否则参数查询）（若PageSize=0，默认为100）</para>
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetParamEqualAsync(QueryParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            if (string.IsNullOrWhiteSpace(parameter.field))  // Field参数为空，按分页全条件查询
                return await tDollDB.GetExpressionAllAsync(exp = (x) => true, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
            else
            {
                switch (parameter.field)
                {
                    case "Name":
                        exp = (x) => x.Name.Equals(parameter.search);
                        break;
                    case "Rarity":
                        exp = (x) => x.Rarity == int.Parse(parameter.search);
                        break;
                    case "Type":
                        exp = (x) => x.Type == (TDoll.TDollType)int.Parse(parameter.search);
                        break;
                    default:
                        return new APIResponse(StatusCode.Select_Wrong_Equal, "该字段无法使用匹配查询");
                }
                return await tDollDB.GetExpressionAllAsync(exp, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
            }
        }

        /// <summary>
        /// 得到某一ID的战术人形元组。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetIDAsync(int id) { return await tDollDB.GetIDAsync(id); }
    }
}
