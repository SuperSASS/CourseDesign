
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.Interfaces
{
    public interface ITDollService : IBaseService<TDollDTO>
    {
        #region 继承的基本方法
        //public new Task<APIResponse<ImagePlanDTO>> Add(int user_id, ImagePlanDTO imagePlanDTO);
        //public new Task<APIResponse> Delete(int id);
        //public new Task<APIResponse<ImagePlanDTO>> GetID(int id);
        //public new Task<APIResponse<ImagePlanDTO>> Update(int user_id, ImagePlanDTO imagePlanDTO);
        #endregion

        /// <summary>
        /// [GET]查询包含某条件的人形
        /// </summary>
        /// <param name="parameter"><see cref="GETParameter"/>类型的查询条件，具体含义用GETParameter(){}查询。</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 查询的分页结果集，类型为<see cref="PagedList{TDollDTO}"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<PagedList<TDollDTO>>> GetParamContain(GETParameter parameter);

        /// <summary>
        /// [GET]查询匹配某条件的人形
        /// </summary>
        /// <param name="parameter"><see cref="GETParameter"/>类型的查询条件，具体含义用GETParameter(){}查询。</param>
        /// <returns>
        /// <see cref="APIResponse{PagedList{TDollDTO}}"/>类型消息
        /// <list type="bullet">
        /// <item>Result: 查询的分页结果集，类型为<see cref="PagedList{TDollDTO}"/></item>
        /// <item>Status: 约定的<see cref="APIStatusCode"/>类型状态码</item>
        /// <item>Message: API返回的消息</item>
        /// </list>
        /// </returns>
        public Task<APIResponse<PagedList<TDollDTO>>> GetParamEqual(GETParameter parameter);
    }
}
