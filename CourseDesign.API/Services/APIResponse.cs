using CourseDesign.Shared;

namespace CourseDesign.API.Services
{
    /// <summary>
    /// API的基本返回结果
    /// </summary>
    public class APIResponse
    {
        public object Result { get; set; }
        public APIStatusCode Status { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 操作成功，无返回值
        /// </summary>
        public APIResponse()
        {
            Result = default;
            Status = APIStatusCode.Success;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作成功，返回元组结果
        /// </summary>
        /// <param name="result">返回的元组</param>
        public APIResponse(object result)
        {
            Result = result;
            Status = APIStatusCode.Success;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作失败，返回失败消息
        /// </summary>
        /// <param name="message">失败提示消息</param>
        /// <param name="status"></param>
        public APIResponse(APIStatusCode status, string message)
        {
            Result = null;
            Status = status;
            Message = message;
        }
    }
}
