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

        // ID查询
        public async Task<APIResponse<TDollDTO>> GetID(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetID?id" + id;
            return await Client.ExecuteAsync<TDollDTO>(request);
        }

        // 参数包含查询
        // 注意！这里必须用PagedList类型，不能自作主张用成List！……
        // 依次参数要用?x=y的形式。

        public async Task<APIResponse<PagedList<TDollDTO>>> GetParamContain(GETParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetParamContain"
                + $"?user_id={parameter.user_id}"
                + $"&search={parameter.search}"
                + $"&field={parameter.field}"
                + $"&page_index={parameter.page_index}"
                + $"&page_size={parameter.page_size}";
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }

        // 参数匹配查询
        public async Task<APIResponse<PagedList<TDollDTO>>> GetParamEqual(GETParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetParamContain"
                + $"?user_id={parameter.user_id}"
                + $"&search={parameter.search}"
                + $"&field={parameter.field}"
                + $"&page_index={parameter.page_index}"
                + $"&page_size={parameter.page_size}";
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }
    }
}
