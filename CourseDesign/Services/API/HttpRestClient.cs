using CourseDesign.Services.API.Requests;
using CourseDesign.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace CourseDesign.Services.API
{
    public class HttpRestClient
    {
        private readonly string APIUrl;
        private readonly RestClient Client;

        // 通过依赖注入，自动传入apiUrl，并生成好HTTP服务端
        public HttpRestClient(string apiUrl)
        {
            APIUrl = apiUrl;           // 在App.cs中依赖注入传来的
            Client = new RestClient(); // 生成服务端
        }

        #region 两种HTTP请求方法
        /// <summary>
        /// 执行请求的通用方法，为无返回Result值的返回类型
        /// </summary>
        /// <param name="baseRequest">请求内容</param>
        /// <returns></returns>
        public async Task<APIResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            try
            {
                // 生成请求，添加请求类型
                var request = new RestRequest(baseRequest.Method); // Debug记录：这里生成的时候要带请求方式，否则默认为GET，导致方式错误
                request.AddHeader("Content-Type", baseRequest.ContentType);
                // 添加请求参数【这里的parameter是个object，然后转换为json格式
                if (baseRequest.Parameter != null)
                    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                // 得到并执行请求
                Client.BaseUrl = new Uri(APIUrl + baseRequest.Route);
                var response = await Client.ExecuteAsync(request);
                if (!response.IsSuccessful) // 不成功
                    throw new Exception(response.ErrorMessage);
                return JsonConvert.DeserializeObject<APIResponse>(response.Content);
            }
            catch (Exception ex)
            {
                return new APIResponse() { Status = APIStatusCode.Unknown_Error, Message = ex.Message };
            }
        }

        /// <summary>
        /// 执行请求的通用方法，为带返回Result值的返回类型
        /// </summary>
        /// <param name="baseRequest">请求内容</param>
        /// <returns></returns>
        public async Task<APIResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            try
            {
                // 生成请求，添加请求类型
                var request = new RestRequest(baseRequest.Method);
                request.AddHeader("Content-Type", baseRequest.ContentType);
                // 添加请求参数
                if (baseRequest.Parameter != null)
                    request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                // 执行请求
                Client.BaseUrl = new Uri(APIUrl + baseRequest.Route);
                var response = await Client.ExecuteAsync(request);
                if (!response.IsSuccessful) // 不成功
                    throw new Exception(response.ErrorMessage);
                return JsonConvert.DeserializeObject<APIResponse<T>>(response.Content);
            }
            catch (Exception ex)
            {
                return new APIResponse<T>() { Status = APIStatusCode.Unknown_Error, Message = ex.Message };
            }
        }
        #endregion
    }
}