using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Common.Classes;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.API.Requests;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.ClassServices
{
    /// <summary>
    /// 这里实现，增删改都在基本的服务BaseService里实现的，所以直接继承即可。
    /// 还要实现查的。
    /// <para>本来BaseService就继承于IBaseService，IMagePlanService也继承于IBaseService，感觉有点多余。采用双重继承的原因可能是降低类之间的耦合性【？……</para>
    /// </summary>
    public class ImagePlanService : BaseHTTPService<ImagePlanDTO>, IImagePlanService
    {
        // 通过依赖注入传入所拥有的HTTP客户端client
        public ImagePlanService(HttpRestClient client) : base(client, "ImagePlan") { } //第二个参数代表的控制器名字 ServiceName


        // 查用户所有
        public async Task<APIResponse<PagedList<ImagePlanDTO>>> GetAllForUser(int user_id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAllForUser?user_id=" + user_id; // 用户user_id直接加载路由里
            return await Client.ExecuteAsync<PagedList<ImagePlanDTO>>(request);
        }
    }
}
