using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseDesign.Command.Classes.TasksBase;

namespace CourseDesign.Command.Classes
{
    /// <summary>
    /// 图片计划类，特指计划打捞某位战术人形。
    /// <para>继承与计划基类<c>TasksBase</c>，具有四个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 计划编号</item>
    /// <item><c>Type</c> - 计划类型</item>
    /// <item><c>Status</c> - 计划状态</item>
    /// <item><c>T_Doll_ID</c> - 计划打捞战术人形的ID</item>
    /// </list>
    internal class ImageTasksClass : TasksBase
    {
        private int T_Doll_id;

        public int T_Doll_ID
        {
            get { return T_Doll_id; }
            set { T_Doll_id = value; }
        }

        /// <summary>
        /// 图片计划类的构造函数，计划种类Type在这里传image给基类
        /// </summary>
        /// <param name="id">计划ID（基类的属性）</param>
        /// <param name="status">计划状态（0表示未完成，1表示已完成）（基类的属性）</param>
        /// <param name="T_Doll_id">计划打捞战术人形的ID</param>
        public ImageTasksClass(int id, bool status, int T_Doll_id) : base(id, typeEnum.image, status)
        {
            T_Doll_ID = T_Doll_id;
        }
    }
}
