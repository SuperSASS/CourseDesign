using CourseDesign.Services.Interfaces;
using CourseDesign.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services
{
    /// <summary>
    /// 这里实现，因为就是基本的服务BaseService里实现的，所以直接继承即可。
    /// <para>本来BaseService就继承于IBaseService，IMageTaskService也继承于IBaseService，感觉有点多余。采用双重继承的原因可能是降低类之间的耦合性【？……</para>
    /// </summary>
    public class ImageTaskService : BaseService<ImageTaskDTO>, IImageTaskService
    {
        public ImageTaskService(HttpRestClient client) : base(client, "ImageTask") { }
    }
}
