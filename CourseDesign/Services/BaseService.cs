using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 通用的服务层
    /// <para>本节理解不是很深刻，先照着范例写</para>
    /// </summary>
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        public readonly HttpRestClient Client;
        public readonly string ServiceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            Client = client;
            ServiceName = serviceName;
        }
        // 以下各种方法自己实现

        /// <summary>
        /// 异步上传
        /// </summary>
        /// <param name="entity">上传的实体</param>
        /// <returns>API的返回消息APIResopnse，返回操作成功的实体</returns>
        public async Task<APIResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{ServiceName}/Add";
            request.Parameter = entity;
            return await Client.ExecuteAsync<TEntity>(request);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="id">删除的id</param>
        /// <returns>API的返回消息APIResopnse，没有实体返回</returns>
        public async Task<APIResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.DELETE;
            request.Route = $"api/{ServiceName}/Delete?id={id}"; // 直接传id即可
            return await Client.ExecuteAsync(request); // 告诉结果即可，不用返回实体
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体更新后的实体</param>
        /// <returns>API的返回消息APIResopnse，返回操作成功的实体</returns>
        public async Task<APIResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{ServiceName}/Update";
            request.Parameter = entity;
            return await Client.ExecuteAsync<TEntity>(request);
        }
    }
}
