using RestSharp;

namespace CourseDesign.Services.API.Requests
{
    /// <summary>
    /// 请求方式
    /// </summary>
    /// <param name="method">请求方式，在RestSharp.Method里</param>
    /// <param name="route">路由，就是访问的地址：$"api/{ServiceName}/..."</param>
    /// <param name="contentType">内容模式，一般就是json</param>
    /// <param name="parameter">参数</param>
    public class BaseRequest
    {
        public Method Method { get; set; }
        public string Route { get; set; }
        public string ContentType { get; set; } = "application/json";
        public object Parameter { get; set; } // 注意！这个Parameter只能适用于POST方法，GET方法请一个个?进去
    }
}
