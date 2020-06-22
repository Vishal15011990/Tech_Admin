using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tech_Admin.Models.DbOperation
{
    public class MasterAdminModel
    {
        public int Id { get; set; }
        [Required]
        public string AdminName { get; set; }

        [Required]

        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
    }
}