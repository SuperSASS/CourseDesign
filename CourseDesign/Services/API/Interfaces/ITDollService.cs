
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.API.Interfaces
{
    public interface ITDollService : IBaseService<TDollDTO>
    {
        #region 继承的基本方法
        //public new Task<APIResponse<ImagePlanDTO>> Add(ImagePlanDTO imagePlanDTO);
        //public new Task<APIResponse> Delete(int id);
        //public new Task<APIResponse<ImagePlanDTO>> GetID(int id);
        //public new Task<APIResponse<ImagePlanDTO>> Update(ImagePlanDTO imagePlanDTO);
        #endregion

        #region 额外的方法
        /// <summary>
        /// [POST]增加某用户拥有的战术人形
        /// </summary>
        /// <param name="entity"><see cref="TDollObtainDTO"/>类型，代表某用户ID拥有新的人形ID。</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 操作成功后返回增加的结果，类型为<see cref="TDollDTO"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<TDollDTO>> AddUserObtain(TDollObtainDTO entity);

        /// <summary>
        /// [GET]查询包含某条件的人形
        /// </summary>
        /// <param name="param"><see cref="GETParameter"/>类型的查询条件，具体含义用GETParameter(){}查询。</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 查询的分页结果集，类型为<see cref="PagedList{TDollDTO}"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<PagedList<TDollDTO>>> GetUserAndParamContain(GETParameter param);

        /// <summary>
        /// [GET]查询匹配某条件的人形
        /// </summary>
        /// <param name="praram"><see cref="GETParameter"/>类型的查询条件，具体含义用GETParameter(){}查询。</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 查询的分页结果集，类型为<see cref="PagedList{TDollDTO}"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<PagedList<TDollDTO>>> GetUserAndParamEqual(GETParameter praram);
        #endregion
    }
}
