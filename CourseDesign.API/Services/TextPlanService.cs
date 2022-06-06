using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Services.Interfaces;
using CourseDesign.API.Services.Response;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CourseDesign.API.Services.Response.APIResponseInner;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// TextPlan（计划列表的图片类计划）接口的实现
    /// </summary>
    public class TextPlanService : ITextPlanService
    {
        private readonly BaseDBService<TextPlan> textDB;
        private readonly IMapper mapper;
        public TextPlanService(IUnitOfWork unitOfWork, IMapper _mapper) { textDB = new BaseDBService<TextPlan>(unitOfWork); mapper = _mapper; }

        // 增
        public async Task<APIResponseInner> AddAsync(TextPlanDTO dtoEntity) { return await textDB.AddAsync(mapper.Map<TextPlan>(dtoEntity)); }

        // 删
        public async Task<APIResponseInner> DeleteAsync(int id) { return await textDB.DeleteAsync(id); }

        // 查询某用户的所有文本计划
        public async Task<APIResponseInner> GetAllForUserAsync(int user_id)
        {
            Expression<Func<TextPlan, bool>> exp;
            exp = (x) => x.UserID == user_id;
            return await textDB.GetExpressionAllPagedAsync(exp);
        }

        // 按条件查询某用户的文本计划
        public async Task<APIResponseInner> GetParamForUserAsync(GETParameter parameter)
        {
            Expression<Func<TextPlan, bool>> exp;
            if (string.IsNullOrWhiteSpace(parameter.search))  // Search参数为空，代表全条件查询
                exp = (x) => x.UserID == parameter.user_id;
            else
                switch (parameter.field)
                {
                    case "Title": // 按标题查询
                        exp = (x) => x.UserID == parameter.user_id && x.Title.Contains(parameter.search);
                        break;
                    case "Content": // 按内容查询
                        exp = (x) => x.UserID == parameter.user_id && x.Content.Contains(parameter.search);
                        break;
                    case "Status": // 按内容查询
                        exp = (x) => x.UserID == parameter.user_id && x.Status.ToString() == parameter.search;
                        break;
                    default:
                        return new APIResponseInner(APIStatusCode.Select__Wrong_filed, "该字段无法使用包含查询");
                }
            return await textDB.GetExpressionAllPagedAsync(exp, parameter.page_index, parameter.page_size == 0 ? 100 : parameter.page_size);

            // 下面方法有问题，看看后面能不能实现
            //return await textDB.GetExpressionAsync(predicate: x =>
            //    x.UserID == user_id && x.GetType().GetField(parameter.Field).GetValue(x).ToString().Contains(parameter.Search));
        }

        // 改
        public async Task<APIResponseInner> UpdateAsync(TextPlanDTO dtoEntity) { return await textDB.UpdateAsync(mapper.Map<TextPlan>(dtoEntity)); }
    }
}
