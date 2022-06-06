namespace CourseDesign.Shared
{
    /// <summary>
    /// 通讯状态码
    /// </summary>
    public enum APIStatusCode { Success, Select_Wrong_filed, Select_Wrong_Equal, Get_Wrong_Account_or_Password, Get_Account_Haven, Add_Failed, Delete_Failed, Update_Failed, Update_Not_Haven, Unknown_Error }
    
    /// <summary>
    /// 基本的返回消息，不需要执行结果Result（但实际上还是写了，我觉得可删）
    /// </summary>
    public class APIResponse
    {
        public object Result { get; set; } // TODO: 删了这个，更规范
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
    }

    /// <summary>
    /// 支持返回执行结果Result的返回消息
    /// </summary>
    /// <typeparam name="T">泛型模板</typeparam>
    public class APIResponse<T>
    {
        public T Result { get; set; }
        public APIStatusCode Status { get; set; }
        public string Message { get; set; }
    }





    // 来自API的


}
