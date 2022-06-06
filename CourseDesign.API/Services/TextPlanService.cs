using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CourseDesign.API.Services.APIResponse;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// TextPlan（计划列表的图片类计划）接口的实现
    /// </summary>
    public class TextPlanService : ITextPlanService
    {
        private readonly BasicSQLService<TextPlan> textDB;
        private readonly IMapper mapper;
        public TextPlanService(IUnitOfWork unitOfWork, IMapper _mapper) { textDB = new BasicSQLService<TextPlan>(unitOfWork); mapper = _mapper; }

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，异步添加元组dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所要增添的<see cref="TextPlan"/>类型元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> AddAsync(TextPlanDTO dtoEntity) { return await textDB.AddAsync(mapper.Map<TextPlan>(dtoEntity)); }

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，异步删除ID为id的元组。
        /// </summary>
        /// <param name="id">要删除元组的ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> DeleteAsync(int id) { return await textDB.DeleteAsync(id); }

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，获取用户ID为"user_id"的所有文字计划。
        /// </summary>
        /// <param name="user_id">传来的<see cref="APIResponse"/>用户ID</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetAllForUserAsync(int user_id)
        {
            Expression<Func<TextPlan, bool>> exp;
            exp = (x) => x.UserID.Equals(user_id);
            return await textDB.GetExpressionAllAsync(exp);
        }

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，获取用户ID为"user_id"，且满足parameter条件的所有元组，并分页展示。
        /// <para>条件为：单字段包含（对于状态来说是匹配）</para>
        /// </summary>
        /// <param name="parameter">传来的<see cref="APIResponse"/>类型参数（若匹配<see cref="TextPlan.Status"/>，Search需要用<c>true/false</c>）</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> GetParamForUserAsync(int user_id, QueryParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.Search))  // Search参数为空，代表全条件查询
                return await textDB.GetAllAsync();
            else
            {
                Expression<Func<TextPlan, bool>> exp;
                switch (parameter.Field)
                {
                    case "Title": // 按标题查询
                        exp = (x) => x.UserID == user_id && x.Title.Contains(parameter.Search);
                        break;
                    case "Content": // 按内容查询
                        exp = (x) => x.UserID == user_id && x.Content.Contains(parameter.Search);
                        break;
                    case "Status": // 按内容查询
                        exp = (x) => x.UserID == user_id && x.Status.ToString() == parameter.Search;
                        break;
                    default:
                        return new APIResponse(APIStatusCode.Select_Wrong_filed, "该字段无法使用包含查询");
                }
                return await textDB.GetExpressionAllAsync(exp, parameter.PageIndex, parameter.PageSize == 0 ? 100 : parameter.PageSize);
            }

            // 下面方法有问题，看看后面能不能实现
            //return await textDB.GetExpressionAsync(predicate: x =>
            //    x.UserID == user_id && x.GetType().GetField(parameter.Field).GetValue(x).ToString().Contains(parameter.Search));
        }

        /// <summary>
        /// 在<see cref="TextPlan"/>表中，修改元组"dtoEntity。
        /// </summary>
        /// <param name="dtoEntity">所修改的新元组</param>
        /// <returns>执行操作返回的消息 - <see cref="APIResponse"/></returns>
        public async Task<APIResponse> UpdateAsync(TextPlanDTO dtoEntity) { return await textDB.UpdateAsync(mapper.Map<TextPlan>(dtoEntity)); }
    }
}
