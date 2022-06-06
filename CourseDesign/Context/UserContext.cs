using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Context
{
    public class UserContext
    {
        private int userId;

        public int UserID
        {
            get { return userId; }
            set { userId = value; }
        }

    }
}
