using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.API.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.ClassServices
{
    public class TDollService : BaseHTTPService<TDollDTO>, ITDollService
    {
        // 通过依赖注入传入所拥有的HTTP客户端client
        public TDollService(HttpRestClient client) : base(client, "TDoll") { }

        // 增添用户拥有人形
        public async Task<APIResponse<TDollDTO>> AddUserObtain(TDollObtainDTO entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{ServiceName}/AddUserObtain";
            request.Parameter = entity;
            return await Client.ExecuteAsync<TDollDTO>(request);
        }

        // 参数包含查询
        // 注意！这里必须用PagedList类型，不能自作主张用成List！……
        // 依次参数要用?x=y的形式。
        public async Task<APIResponse<PagedList<TDollDTO>>> GetUserAndParamContain(GETParameter param)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetUserAndParamContain"
                + $"?user_id={param.user_id}"
                + $"&search={param.search}"
                + $"&field={param.field}"
                + $"&page_index={param.page_index}"
                + $"&page_size={param.page_size}";
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }

        // 参数匹配查询
        public async Task<APIResponse<PagedList<TDollDTO>>> GetUserAndParamEqual(GETParameter param)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetUserAndParamEqual"
                + $"?user_id={param.user_id}"
                + $"&search={param.search}"
                + $"&field={param.field}"
                + $"&page_index={param.page_index}"
                + $"&page_size={param.page_size}";
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }
    }
}
