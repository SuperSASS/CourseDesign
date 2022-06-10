using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
using CourseDesign.Services.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    public class TDollService : BaseHTTPService<TDollDTO>, ITDollService
    {
        public TDollService(HttpRestClient client) : base(client, "TDoll") { }

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
