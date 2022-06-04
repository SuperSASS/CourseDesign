namespace CourseDesign.API.Services
{
    /// <summary>
    /// API的基本返回结果
    /// </summary>
    public class APIResponse
    {
        public object Result { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 操作成功，无返回值
        /// </summary>
        public APIResponse()
        {
            Result = null;
            Status = true;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作成功，返回元组结果
        /// </summary>
        /// <param name="result">返回的元组</param>
        public APIResponse(object result)
        {
            Result = result;
            Status = true;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作失败，返回失败消息
        /// </summary>
        /// <param name="message">失败提示消息</param>
        /// <param name="status">默认为失败（也可以重写为操作成功返回消息）</param>
        public APIResponse(string message, bool status = false)
        {
            Result = null;
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="result">返回元组结果</param>
        /// <param name="status">操作状态</param>
        /// <param name="message">返回消息</param>
        public APIResponse(object result, bool status, string message)
        {
            Result = result;
            Status = status;
            Message = message;
        }
    }
}
