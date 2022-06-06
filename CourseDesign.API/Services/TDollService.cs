using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CourseDesign.API.Context.TDoll;

namespace CourseDesign.API.Services
{
    public class TDollService : ITDollService
    {
        private readonly BasicDBService<TDoll> tDollDB;
        private readonly IMapper mapper;

        public TDollService(IUnitOfWork unitOfWork, IMapper _mapper) { tDollDB = new BasicDBService<TDoll>(unitOfWork); mapper = _mapper; }

        // 按条件包含查询人形
        public async Task<APIResponseInner> GetParamContainAsync(QueryParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            //if (parameter.user_id != null) // TODO: 3 - 传了用户ID进来，只查该用户的
            //{ }
            //else
            {
                if (string.IsNullOrWhiteSpace(parameter.search))  // Search参数为空，代表按分页全条件查询
                    return await tDollDB.GetExpressionAllPagedAsync(exp = (x) => true, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
                else
                {
                    switch (parameter.field)
                    {
                        case "Name":
                            exp = (x) => x.Name.Contains(parameter.search);
                            break;
                        default:
                            return new APIResponseInner(APIStatusCode.Select__Wrong_filed, "该字段无法使用包含查询");
                    }
                    return await tDollDB.GetExpressionAllPagedAsync(exp, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
                }
            }
        }

        // 按条件匹配查询人形
        public async Task<APIResponseInner> GetParamEqualAsync(QueryParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            if (string.IsNullOrWhiteSpace(parameter.field))  // Field参数为空，按分页全条件查询
                return await tDollDB.GetExpressionAllPagedAsync(exp = (x) => true, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
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
                        exp = (x) => x.Type == (TDollType)int.Parse(parameter.search);
                        break;
                    default:
                        return new APIResponseInner(APIStatusCode.Select_Wrong_Equal, "该字段无法使用匹配查询");
                }
                return await tDollDB.GetExpressionAllPagedAsync(exp, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);
            }
        }

        public async Task<APIResponseInner> GetIDAsync(int id) { return await tDollDB.GetIDAsync(id); }
    }
}
