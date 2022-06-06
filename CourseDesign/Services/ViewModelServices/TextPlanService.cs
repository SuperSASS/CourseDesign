using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Common.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Services.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.ViewModelServices
{
    public class TextPlanService : BaseHTTPService<TextPlanDTO>, ITextPlanService
    {
        // 通过依赖注入传入所拥有的HTTP客户端client
        public TextPlanService(HttpRestClient client) : base(client, "TextPlan") { } //第二个参数代表的控制器名字 ServiceName

        // 查用户所有的
        public async Task<APIResponse<PagedList<TextPlanDTO>>> GetAllForUser(int user_id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAllForUser?user_id=" + user_id; // 用户user_id直接加载路由里
            return await Client.ExecuteAsync<PagedList<TextPlanDTO>>(request);
        }

        // 查用户满足条件的
        public async Task<APIResponse<PagedList<TextPlanDTO>>> GetParamContainForUser(GETParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetParamContainForUser"
                + $"?user_id={parameter.user_id}"
                + $"&search={parameter.search}"
                + $"&field={parameter.field}"
                + $"&page_index={parameter.page_index}"
                + $"&page_size={parameter.page_size}";
            return await Client.ExecuteAsync<PagedList<TextPlanDTO>>(request);
        }
    }
}
