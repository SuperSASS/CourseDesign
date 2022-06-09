using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using CourseDesign.Context;
using CourseDesign.Shared.DTOs;
using Prism.Commands;
using System.Collections.Generic;

namespace CourseDesign.Common.Classes
{
    /// <summary>
    /// 图片计划类，特指计划打捞某位战术人形。
    /// <para>继承与计划基类<c>PlansBase</c>，具有四个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 计划编号</item>
    /// <item><c>Type</c> - 计划类型</item>
    /// <item><c>Status</c> - 计划状态</item>
    /// <item><c>TDoll_ID</c> - 计划打捞战术人形的ID</item>
    /// </list>
    public class ImagePlanClass : PlanBase
    {
        private int? tDoll_ID;
        private TDollClass tDoll;

        public int? TDoll_ID { get { return tDoll_ID; } set { tDoll_ID = value; } }

        /// <summary>
        /// 根据ID找到的这条计划的人形类用于展示各种信息
        /// </summary>
        public TDollClass TDoll { get { return tDoll; } set { tDoll = value; } }

        /// <summary>
        /// 图片计划类的构造函数，计划种类Type在这里传image给基类
        /// </summary>
        /// <param name="id">计划ID（基类的属性）</param>
        /// <param name="status">计划状态（0表示未完成，1表示已完成）（基类的属性）</param>
        /// <param name="tDoll_ID">计划打捞战术人形的ID</param>
        public ImagePlanClass(int id, bool status, int? tDoll_ID) : base(id, PlanType.Image, status)
        {
            TDoll_ID = tDoll_ID;
            if (tDoll_ID != null) TDoll = TDollsContext.GetTDoll((int)tDoll_ID);
        }

        /// <summary>
        /// 与DTO的类型转换，会缺少CreateDate（不过传输过去并不需要）
        /// </summary>
        /// <param name="APPEntity">APP中的实体类型</param>
        /// <param name="userID">该实体属于哪个用户</param>
        public ImagePlanDTO ConvertDTO(ImagePlanClass APPEntity, int userID)
        {
            return new ImagePlanDTO()
            {
                ID = APPEntity.ID,
                CreateDate = null,
                Status = APPEntity.Status,
                TDoll_ID = (int)APPEntity.TDoll_ID,
                Type = (PlanDTO.PlanType)APPEntity.Type,
                UserID = userID
            };
        }
    }
}
