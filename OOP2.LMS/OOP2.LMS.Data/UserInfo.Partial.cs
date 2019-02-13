using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP2.LMS.Data
{
    public partial class UserInfo
    {
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public string DepartmentName
        {
            get
            {
                if (this.Department == null)
                    return "";
                else
                {
                    return this.Department.Name;
                }
            }
        }
    }
}
