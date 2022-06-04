using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 通用的服务层
    /// <para>本节理解不是很深刻，先照着范例写，后面了解下where TEtiyu:class是啥；和泛型<>这个概念【那个<<<>>>是怎么套三层的？！……</para>
    /// </summary>
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient Client;
        private readonly string ServiceName;

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
        /// 返回符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>
        public async Task<APIResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAll?pageIndex={parameter.PageIndex}"
                + $"&pageSize={parameter.PageSize}"
                + $"$search={parameter.Search}";
            return await Client.ExecuteAsync<PagedList<TEntity>>(request);
        }

        /// <summary>
        /// 返回ID为id的实体
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体</returns>
        public async Task<APIResponse<TEntity>> GetFirstDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/Get?id={id}";
            return await Client.ExecuteAsync<TEntity>(request);
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
