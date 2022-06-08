using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Constants;
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
        private readonly BaseDBService<TDoll> tDollDB;
        private readonly IMapper mapper;

        public TDollService(IUnitOfWork unitOfWork, IMapper _mapper) { tDollDB = new BaseDBService<TDoll>(unitOfWork); mapper = _mapper; }

        #region 方法实现
        // ID查询
        public async Task<APIResponseInner> GetIDAsync(int id) { return await tDollDB.GetIDAsync(id); }

        // 按条件包含查询人形【注意，这里只是图鉴查询，如果用户查询调用GetUserParamContainObtainAsync
        public async Task<APIResponseInner> GetParamContainAsync(GETParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            //if (parameter.user_id != null) // TODO: 3 - 传了用户ID进来，只查该用户的
            //{ }
            //else
            {
                if (string.IsNullOrWhiteSpace(parameter.search))  // Search参数为空，代表按分页全条件查询
                    return await tDollDB.GetExpressionAllPagedAsync(exp = (x) => true, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize);
                else
                {
                    switch (parameter.field)
                    {
                        case "Name":
                            exp = (x) => x.Name.Contains(parameter.search);
                            break;
                        default:
                            return new APIResponseInner(APIStatusCode.Select_Wrong_filed, "该字段无法使用包含查询");
                    }
                    return await tDollDB.GetExpressionAllPagedAsync(exp, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize);
                }
            }
        }

        // 按条件匹配查询人形
        public async Task<APIResponseInner> GetParamEqualAsync(GETParameter parameter)
        {
            Expression<Func<TDoll, bool>> exp;
            if (string.IsNullOrWhiteSpace(parameter.field))  // Field参数为空，按分页全条件查询
                return await tDollDB.GetExpressionAllPagedAsync(exp = (x) => true, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize);
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
                return await tDollDB.GetExpressionAllPagedAsync(exp, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize);
            }
        }
        #endregion
    }
}
