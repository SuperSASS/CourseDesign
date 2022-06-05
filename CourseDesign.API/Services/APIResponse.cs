namespace CourseDesign.API.Services
{
    /// <summary>
    /// API的基本返回结果
    /// </summary>
    public class APIResponse
    {
        public enum StatusCode { Success, Select__Wrong_filed, Select_Wrong_Equal, Get_Wrong_Account_or_Password , Get_Account_Haven , Add_Failed, Delete_Failed, Update_Failed, Update_Not_Haven, Unknown_Error }
        public object Result { get; set; }
        public StatusCode Status { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 操作成功，无返回值
        /// </summary>
        public APIResponse()
        {
            Result = default;
            Status = 0;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作成功，返回元组结果
        /// </summary>
        /// <param name="result">返回的元组</param>
        public APIResponse(object result)
        {
            Result = result;
            Status = 0;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作失败，返回失败消息
        /// </summary>
        /// <param name="message">失败提示消息</param>
        /// <param name="status"></param>
        public APIResponse(StatusCode status, string message)
        {
            Result = default;
            Status = status;
            Message = message;
        }
    }
}
