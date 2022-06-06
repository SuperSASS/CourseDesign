using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
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
    public class TDollService : BaseService<TDollDTO>, ITDollService
    {
        public TDollService(HttpRestClient client) : base(client, "TDoll") { }

        public async Task<APIResponse<TDollDTO>> GetID(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetID?id" + id;
            return await Client.ExecuteAsync<TDollDTO>(request);
        }

        // 注意！这里必须用PagedList类型，不能自作主张用成List！……

        public async Task<APIResponse<PagedList<TDollDTO>>> GetParamContain(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetParamContain";
            request.Parameter = parameter;
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }

        public async Task<APIResponse<PagedList<TDollDTO>>> GetParamEqual(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetParamEqual";
            request.Parameter = parameter;
            return await Client.ExecuteAsync<PagedList<TDollDTO>>(request);
        }
    }
}
