using CourseDesign.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    public class HttpRestClient
    {
        private readonly string APIUrl;
        private readonly RestClient Client;

        public HttpRestClient(string apiUrl)
        {
            APIUrl = apiUrl;
            Client = new RestClient();
        }

        /// <summary>
        /// 执行请求的通用方法
        /// </summary>
        /// <param name="baseRequest">请求内容</param>
        /// <returns></returns>
        public async Task<APIResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            // 生成请求，添加请求类型
            var request = new RestRequest();
            request.AddHeader("Content-Type", baseRequest.ContentType);
            // 添加请求参数，这里的parameter是个object，自动转换为json格式
            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            // 得到并执行请求
            Client.BaseUrl = new Uri(APIUrl + baseRequest.Route);
            var response = await Client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<APIResponse>(response.Content);
        }

        /// <summary>
        /// 执行请求的通用方法，支持泛型的返回类型
        /// </summary>
        /// <param name="baseRequest">请求内容</param>
        /// <returns></returns>
        public async Task<APIResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            // 生成请求，添加请求类型
            var request = new RestRequest();
            request.AddHeader("Content-Type", baseRequest.ContentType);
            // 添加请求参数
            if (baseRequest.Parameter != null) // TODO: 0-0 - 问题可能在这个名字上
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            // 执行请求
            Client.BaseUrl = new Uri(APIUrl + baseRequest.Route);
            var response = await Client.ExecuteAsync(request);
            var re = JsonConvert.DeserializeObject<APIResponse<T>>(response.Content);
            return JsonConvert.DeserializeObject<APIResponse<T>>(response.Content);
        }
    }
}
