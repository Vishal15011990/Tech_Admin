using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tech_Admin.Models.DbOperation
{
    public class RoleM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public System.Guid Createdby { get; set; }
        public System.DateTime Createdon { get; set; }
        public bool isDelete { get; set; }
        public bool isActive { get; set; }
    }
}