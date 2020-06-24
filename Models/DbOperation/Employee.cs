using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tech_Admin.Models.DbOperation
{
    public class Employee
    {
        public int EmpId { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public System.DateTime Date_Of_Birth { get; set; }
        [Required]

        public string Phone { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        public int? Country { get; set; }
        [Required]

        public int? State { get; set; }
        [Required]

        public int? City { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string EmailId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModiefiedOn { get; set; }
        public Guid? CreatedbY { get; set; }
        public DateTime? Createdon { get; set; }
        public  CountryModel CountryMd { get; set; }
        public StateModel StateMd { get; set; }
        public CityModel CityMd { get; set; }
        public RoleM RoleMd { get; set; }
        public FileUpload FileuploadMd { get; set; }
    }
}