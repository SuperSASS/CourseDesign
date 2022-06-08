using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using CourseDesign.API.Constants;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CourseDesign.API.Context.TDoll;

namespace CourseDesign.API.Services
{
    public class TDollService : ITDollService
    {
        private readonly BaseDBService<TDoll> tDollDB;
        private readonly BaseDBService<TDollObtain> tDollObtainDB;
        private readonly IMapper mapper;

        public TDollService(IUnitOfWork unitOfWork, IMapper _mapper) { tDollDB = new BaseDBService<TDoll>(unitOfWork); tDollObtainDB = new BaseDBService<TDollObtain>(unitOfWork); mapper = _mapper; }

        // 简化表达式用，代表该用户ID的所拥有人形数据给查出来
        Expression<Func<TDollObtain, bool>> exp_user(int user_id) { return (x) => x.FK_UserID == user_id; }

        #region 方法实现
        // 增加某用户所拥有的战术人形，穿的是TDollObtainDTO类型数据
        public async Task<APIResponseInner> AddUserObtainTDollAsync(TDollObtainDTO dtoEntity) { return await tDollObtainDB.AddAsync(mapper.Map<TDollObtain>(dtoEntity)); }

        // ID查询
        public async Task<APIResponseInner> GetIDAsync(int id) { return await tDollDB.GetIDAsync(id); }

        // 按条件包含查询某用户拥有的人形【若search为null，代表查询用户所有的
        // 注：该方法目前实现方式较为复杂（分别判断了user_id存在的情况，然后是否存在都判断了search是否存在，一共4个分支；以后看看能不能利用Expression简化为2+2的形式……
        public async Task<APIResponseInner> GetUserAndParamContainAsync(GETParameter parameter)
        {
            // 传了用户user_id属性的查询方式
            if (parameter.user_id != null)
            {
                APIResponseInner userTDolls = await tDollObtainDB.GetExpressionAllPagedAsync(exp_user((int)parameter.user_id), parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize); // 先直接得到用户拥有的所有人形
                if (userTDolls.Status != APIStatusCode.Success) // 发生神必异常，直接return，后同
                    return userTDolls;

                // 开始遍历用户的所有拥有的人形，然后作为表达式条件进行单值查询，再增加到最终结果
                List<TDoll> getUserTDolls = new();
                foreach (var userTDoll in (userTDolls.Result as PagedList<TDollObtain>).Items)
                {
                    Expression<Func<TDoll, bool>> exp_param; // 条件查询表达式
                    if (string.IsNullOrWhiteSpace(parameter.search)|| string.IsNullOrWhiteSpace(parameter.field))  // search或field为空，代表按分页对该用户全条件查询，因此直接返回user_tDoll
                        exp_param = (x) => x.ID == userTDoll.FK_TDollID;
                    else
                        switch (parameter.field)
                        {
                            case "Name":
                                {
                                    exp_param = (x) => x.ID == userTDoll.FK_TDollID && x.Name.Contains(parameter.search);
                                    break;
                                }
                            default:
                                return new APIResponseInner(APIStatusCode.Select_Wrong_filed, "该字段无法使用包含查询");
                        }
                    APIResponseInner getTDoll = await tDollDB.GetExpressionSingalAsync(exp_param);
                    if (getTDoll.Status != APIStatusCode.Success)
                        return getTDoll;
                    if (getTDoll.Result != null) // 不为空，代表确实找到，增加到队列
                        getUserTDolls.Add((TDoll)getTDoll.Result);
                }
                return new APIResponseInner(new PagedList<TDoll>(getUserTDolls, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize, 0));//这个indexFrom啊，可能是一个当前起始页（因为前面的页号可以省略，如..., P6, P7这样），这里没怎么用分页所以默认传0
            }
            // 没传用户user_id属性的查询方式
            else
            {
                Expression<Func<TDoll, bool>> exp;
                if (string.IsNullOrWhiteSpace(parameter.search) || string.IsNullOrWhiteSpace(parameter.field))  // search或field参数为空，代表按分页全条件查询
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

        // 按条件匹配查询人形【若search为null，代表查询用户所有的
        // 问题同上，实现较为复杂……
        public async Task<APIResponseInner> GetUserAndParamEqualAsync(GETParameter parameter)
        {
            // 传了用户user_id属性的查询方式
            if (parameter.user_id != null)
            {
                APIResponseInner userTDolls = await tDollObtainDB.GetExpressionAllPagedAsync(exp_user((int)parameter.user_id), parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize); // 先直接得到用户拥有的所有人形
                if (userTDolls.Status != APIStatusCode.Success) // 发生神必异常，直接return，后同
                    return userTDolls;

                // 开始遍历用户的所有拥有的人形，然后作为表达式条件进行单值查询，再增加到最终结果
                List<TDoll> getUserTDolls = new();
                foreach (var userTDoll in (userTDolls.Result as PagedList<TDollObtain>).Items)
                {
                    Expression<Func<TDoll, bool>> exp_param; // 条件查询表达式
                    if (string.IsNullOrWhiteSpace(parameter.search) || string.IsNullOrWhiteSpace(parameter.field))  // Search或field参数为空，代表按分页对该用户全条件查询，因此直接返回user_tDoll
                        exp_param = (x) => x.ID == userTDoll.FK_TDollID;
                    else
                        switch (parameter.field)
                        {
                            case "Name":
                                exp_param = (x) => x.ID == userTDoll.FK_TDollID && x.Name.Equals(parameter.search);
                                break;
                            case "Rarity":
                                exp_param = (x) => x.ID == userTDoll.FK_TDollID && x.Rarity == int.Parse(parameter.search);
                                break;
                            case "Type":
                                exp_param = (x) => x.ID == userTDoll.FK_TDollID && x.Type == (TDollType)int.Parse(parameter.search);
                                break;
                            default:
                                return new APIResponseInner(APIStatusCode.Select_Wrong_Equal, "该字段无法使用匹配查询");
                        }
                    APIResponseInner getTDoll = await tDollDB.GetExpressionSingalAsync(exp_param);
                    if (getTDoll.Status != APIStatusCode.Success)
                        return getTDoll;
                    if (getTDoll.Result != null) // 不为空，代表确实找到，增加到队列
                        getUserTDolls.Add((TDoll)getTDoll.Result);
                }
                return new APIResponseInner(new PagedList<TDoll>(getUserTDolls, parameter.page_index ?? PageConst.PageIndex, parameter.page_size ?? PageConst.PageSize, 0));
            }
            // 没传用户user_id属性的查询方式
            else
            {
                Expression<Func<TDoll, bool>> exp;
                if (string.IsNullOrWhiteSpace(parameter.search) || string.IsNullOrWhiteSpace(parameter.field))  // search或field参数为空，按分页全条件查询
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
        }
        #endregion
    }
}
