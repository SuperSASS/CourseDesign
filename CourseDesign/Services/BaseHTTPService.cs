using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
using CourseDesign.Services.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 通用的服务层，估计作用就是拿来给其他服务重用的
    /// </summary>
    public class BaseHTTPService<DTOEntity> : IBaseService<DTOEntity> where DTOEntity : class
    {
        public readonly HttpRestClient Client;
        public readonly string ServiceName;

        /// <summary>
        /// 基本服务基类的构造函数
        /// </summary>
        /// <param name="client">提供Http服务的客户端</param>
        /// <param name="serviceName">该服务的名称</param>
        public BaseHTTPService(HttpRestClient client, string serviceName)
        {
            Client = client;
            ServiceName = serviceName;
        }

        #region 具体实现的增、删、查ID、改方法
        // 增
        public async Task<APIResponse<DTOEntity>> Add(DTOEntity param)  // 注意：POST协议，参数要为一个序列化的object
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/Add",
                Parameter = param
            };
            return await Client.ExecuteAsync<DTOEntity>(request);
        }

        // 删
        public async Task<APIResponse> Delete(int id)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"api/{ServiceName}/Delete?id={id}" // 直接传id即可
            };
            return await Client.ExecuteAsync(request); // 告诉结果即可，不用返回实体
        }

        // 查ID
        public async Task<APIResponse<DTOEntity>> GetID(int id)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"api/{ServiceName}/Delete?id={id}" // 直接传id即可
            };
            return await Client.ExecuteAsync<DTOEntity>(request);
        }

        // 改
        public async Task<APIResponse<DTOEntity>> Update(DTOEntity param)
        {
            BaseRequest request = new()
            {
                Method = RestSharp.Method.POST,
                Route = $"api/{ServiceName}/Update",
                Parameter = param
            };
            return await Client.ExecuteAsync<DTOEntity>(request);
        }
        #endregion
    }
}
