using CourseDesign.Services.Interfaces;
using CourseDesign.Shared.DTOs;

namespace CourseDesign.Services
{
    /// <summary>
    /// 这里实现，因为就是基本的服务BaseService里实现的，所以直接继承即可。
    /// <para>本来BaseService就继承于IBaseService，IMagePlanService也继承于IBaseService，感觉有点多余。采用双重继承的原因可能是降低类之间的耦合性【？……</para>
    /// </summary>
    public class ImagePlanService : BaseService<ImagePlanDTO>, IImagePlanService
    {
        public ImagePlanService(HttpRestClient client) : base(client, "ImagePlan") { }
    }
}
