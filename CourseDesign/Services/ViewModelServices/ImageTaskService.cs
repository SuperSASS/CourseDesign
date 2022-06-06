﻿using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System.Threading.Tasks;

namespace CourseDesign.Services.ViewModelServices
{
    /// <summary>
    /// 这里实现，增删改都在基本的服务BaseService里实现的，所以直接继承即可。
    /// 还要实现查的。
    /// <para>本来BaseService就继承于IBaseService，IMagePlanService也继承于IBaseService，感觉有点多余。采用双重继承的原因可能是降低类之间的耦合性【？……</para>
    /// </summary>
    public class ImagePlanService : BaseService<ImagePlanDTO>, IImagePlanService
    {
        public ImagePlanService(HttpRestClient client) : base(client, "ImagePlan") { } //第二个参数代表的控制器名字 ServiceName

        /// <summary>
        /// 返回该用户符合条件的所有结果
        /// </summary>
        /// <param name="parameter">查询条件所用的参数</param>
        /// <returns>API的返回消息APIResopnse，返回满足条件的实体页</returns>
        public async Task<APIResponse<PagedList<ImagePlanDTO>>> GetAllForUser(int user_id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/{ServiceName}/GetAllForUser?id=1"; //TODO:2 - 这里直接指定用户1
            return await Client.ExecuteAsync<PagedList<ImagePlanDTO>>(request);
        }
    }
}
