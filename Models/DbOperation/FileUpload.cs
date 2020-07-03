using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Tech_Admin.Models.DbOperation;

namespace Tech_Admin.Models
{
    public class FileUpload2
    {
        public System.Guid UploadId { get; set; }
        public HttpPostedFileBase File_name { get; set; }
        public HttpPostedFileBase File_Path { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? UserRefId { get; set; }
        public Employee EmployeeMD { get; set; }
       
    }
}