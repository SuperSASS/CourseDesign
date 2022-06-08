using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Shared.DTOs
{
    public class TDollObtainDTO : BaseDTO
    {
        private int userId;
        private int tDollId;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// 对应拥有的人形
        /// </summary>
        public int TDollID
        {
            get { return tDollId; }
            set { tDollId = value; }
        }
    }
}
