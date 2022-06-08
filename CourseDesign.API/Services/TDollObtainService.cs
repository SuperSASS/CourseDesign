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
    public class TDollObtainService : ITDollObtainService
    {
        private readonly BaseDBService<TDollObtain> tDollObtainDB;
        private readonly BaseDBService<TDoll> tDollDB; // 由于有多表查询，也需要把TDollDB表拿过来
        private readonly IMapper mapper;

        // 简化表达式用，代表该用户ID的所拥有人形数据给查出来
        Expression<Func<TDollObtain, bool>> exp_user(int user_id) { return (x) => x.FK_UserID == user_id; }

        // 构造函数，由注册依赖自己完成
        public TDollObtainService(IUnitOfWork unitOfWork, IMapper _mapper) { tDollObtainDB = new BaseDBService<TDollObtain>(unitOfWork); tDollDB = new BaseDBService<TDoll>(unitOfWork); mapper = _mapper; }

        // 增用户拥有的战术人形
        public async Task<APIResponseInner> AddUserObtainTDollAsync(TDollObtainDTO dtoEntity) { return await tDollObtainDB.AddAsync(mapper.Map<TDollObtain>(dtoEntity)); }

        // 查用户拥有的所有
        public async Task<APIResponseInner> GetUserAllObtainTDollAsync(int user_id)
        {

            APIResponseInner userTDolls = await tDollObtainDB.GetExpressionAllPagedAsync(exp_user((int)user_id)); // 先直接得到用户拥有的所有人形
            if (userTDolls.Status != APIStatusCode.Success) // 发生神必异常，直接return，后同
                return userTDolls;

            // 开始遍历用户的所有拥有的人形，然后作为表达式条件进行单值查询，再增加到最终结果
            // 因此是按照获得顺序（ID递减）降序排序
            List<TDoll> getUserTDolls = new();
            foreach (var userTDoll in (userTDolls.Result as PagedList<TDollObtain>).Items)
            {
                APIResponseInner getTDoll = await tDollDB.GetExpressionSingalAsync((x) => x.ID == userTDoll.FK_TDollID);
                if (getTDoll.Status != APIStatusCode.Success)
                    return getTDoll;
                if (getTDoll.Result != null) // 不为空，代表确实找到，增加到队列
                    getUserTDolls.Add((TDoll)getTDoll.Result);
            }
            return new APIResponseInner(new PagedList<TDoll>(getUserTDolls, PageConst.PageIndex, PageConst.PageSize, 0));//这个indexFrom啊，可能是一个当前起始页（因为前面的页号可以省略，如..., P6, P7这样），这里没怎么用分页所以默认传0
        }

        // 查用户条件包含
        // 这里涉及到多表查询，因此只能先把用户拥有的查出来，再遍历一个个比对添加
        public async Task<APIResponseInner> GetUserParamContainObtainTDollAsync(GETParameter parameter)
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

        // 查用户条件匹配
        // 直接把TDoll里的复制过来就行
        public async Task<APIResponseInner> GetUserParamEqualObtainTDollAsync(GETParameter parameter)
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
    }
}
