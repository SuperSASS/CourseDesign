using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Command.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.ViewModelServices
{
    public class TextPlanService : BaseService<TextPlanDTO>, ITextPlanService
    {
        public TextPlanService(HttpRestClient client) : base(client, "TextPlan") { } //第二个参数代表的控制器名字 ServiceName


        /// <summary>
        /// 返回该用户符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>

        public async Task<APIResponse<PagedList<TextPlanDTO>>> GetAllForUserAsync(int user_id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAllForUser?user_id=" + user_id; // 用户user_id直接加载路由里
            return await Client.ExecuteAsync<PagedList<TextPlanDTO>>(request);
        }

        public async Task<APIResponse<PagedList<TextPlanDTO>>> GetParamContainForUserAsync(int user_id, QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAllForUser?user_id=" + user_id; // 用户user_id直接加载路由里
            request.Parameter = parameter; 
            return await Client.ExecuteAsync<PagedList<TextPlanDTO>>(request);
        }
    }
}
