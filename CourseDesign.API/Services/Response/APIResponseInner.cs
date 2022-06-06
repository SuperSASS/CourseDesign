using CourseDesign.Shared;

namespace CourseDesign.API.Services.Response
{
    /// <summary>
    /// 在API层内部定义的返回结果
    /// </summary>
    /// 注：与DTO层的并不一样是内部的，因为不考虑各种类的其他属性方法，这里需要抽象成Object【即没有支持泛型返回……
    /// 
    public class APIResponseInner
    {
        #region 属性
        /// <summary>
        /// 操作的结果，为object内类型
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 操作的状态，状态码定义在Shared中（注意：没查询到也为成功，不过Result为null）
        /// </summary>
        public APIStatusCode Status { get; set; }
        /// <summary>
        /// 操作的信息。
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 操作成功，无返回值
        /// </summary>
        public APIResponseInner()
        {
            Result = default;
            Status = APIStatusCode.Success;
            Message = "成功！……";
        }

        /// <summary>
        /// 操作成功，返回元组结果
        /// </summary>
        /// <param name="result">返回的元组</param>
        public APIResponseInner(object result)
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
        public APIResponseInner(APIStatusCode status, string message)
        {
            Result = null;
            Status = status;
            Message = message;
        }
        #endregion
    }
}
