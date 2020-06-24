using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.Entity;
using System.Web.Management;
using System.IO;

namespace Tech_Admin.Models.DbOperation
{
    public class DbOperation
    {
      

        public int CreateEmp(Employee_Master emp)
        {
            using (var context = new EmployeeEntities())
            {
                Employee_Master emp1 = new Employee_Master()
                {
                    Name = emp.Name,
                    Date_Of_Birth = emp.Date_Of_Birth,
                    Phone = emp.Phone,
                    Address = emp.Address,
                    Country=emp.Country_Info.Country_Id,
                    State = emp.State_info.State_Id,
                    City = emp.City_Info.City_Id,
                    EmailId = emp.EmailId,
                    IsActive = true,
                    CreatedbY = Guid.NewGuid(),
                    Createdon = DateTime.UtcNow,
                    RoleId=emp.RoleMaster.RoleId,
                    Password=emp.Password,
                };
                context.Entry(emp1).State = EntityState.Added;
                context.SaveChanges();
                return emp1.EmpId;
            }
        }



        //public File_Upload Uploadfile(FileUpload fup)
        //{
        //    using (var context = new EmployeeEntities())
        //    {
        //        File_Upload fp = new File_Upload()
        //        {
        //            UploadID = Guid.NewGuid(),
        //            IsActive = true,
        //            Created_By = Guid.NewGuid(),
        //            Created_On = DateTime.Now,
        //            File_Name =fup.File.FileName+ Guid.NewGuid(),
        //            File_Size = fup.File.ContentLength,
        //            UserRefId = fup.EmployeeMD.EmpId,
        //            Extension = Path.GetExtension(fup.File.FileName).Substring(1),
        //        };
        //        return fp;
        //    }
        //}


        public int RoleEmp(RoleM rolem)
        {
            using(var context=new EmployeeEntities())
            {
                RoleMaster emp = new RoleMaster()
                {
                    RoleName = rolem.RoleName,
                    isActive = true,
                    Createdby = Guid.NewGuid(),
                    Createdon = DateTime.UtcNow,
                };
                context.Entry(emp).State = EntityState.Added;
                context.SaveChanges();
                return emp.RoleId;
            }
        }
        public int Create(Employee emp)
        {
            using (var context = new EmployeeEntities())
            {
                Employee_Master emp1 = new Employee_Master()
                {
                    Name = emp.Name,
                    Date_Of_Birth = emp.Date_Of_Birth,
                    Phone = emp.Phone,
                    Address = emp.Address,
                    Country = emp.Country,
                    State = emp.State,
                    City = emp.City,
                    EmailId = emp.EmailId,
                    IsActive = true,
                    CreatedbY = Guid.NewGuid(),
                    Createdon = DateTime.UtcNow,
                };
                context.Entry(emp1).State = EntityState.Added;
                context.SaveChanges();
                return emp1.EmpId;
            }
        }

        public List<Employee> Getrecord()
        {
            using (var context = new EmployeeEntities())
            {
                var result = context.Employee_Master.Select(x => new Employee()
                {
                    EmpId = x.EmpId,
                    Name = x.Name,
                    Date_Of_Birth = x.Date_Of_Birth,
                    Phone = x.Phone,
                    Address = x.Address,
                    CountryMd = new CountryModel()
                    {
                        Country_Id = x.Country_Info.Country_Id,
                        Country_name = x.Country_Info.Country_name,
                    },
                    StateMd = new StateModel()
                    {
                        State_Id = x.State_info.State_Id,
                        State_Name = x.State_info.State_Name,
                        Country_RefId = x.State_info.Country_RefId,
                    },
                    CityMd = new CityModel()
                    {
                        City_Id = x.City_Info.City_Id,
                        City_Name = x.City_Info.City_Name,
                        State_RefId = x.City_Info.State_RefId,
                    },
                    RoleMd=new RoleM()
                    {
                        RoleId=x.RoleMaster.RoleId,
                        RoleName=x.RoleMaster.RoleName
                    }
                }).ToList();
                return result;
            }
        }















        public Employee Details(int id)
        {
            using(var context=new EmployeeEntities())
            {
                var result = context.Employee_Master.Where(x => x.EmpId == id).Select(x => new Employee()
                {
                    EmpId = x.EmpId,
                    Name = x.Name,
                    Date_Of_Birth = x.Date_Of_Birth,
                    Phone = x.Phone,
                    Address = x.Address,
                    CountryMd = new CountryModel()
                    {
                        Country_Id = x.Country_Info.Country_Id,
                        Country_name = x.Country_Info.Country_name,
                    },
                    StateMd = new StateModel()
                    {
                        State_Id = x.State_info.State_Id,
                        State_Name = x.State_info.State_Name,
                        Country_RefId = x.State_info.Country_RefId,
                    },
                    CityMd = new CityModel()
                    {
                        City_Id = x.City_Info.City_Id,
                        City_Name = x.City_Info.City_Name,
                        State_RefId = x.City_Info.State_RefId,
                    }
                }).FirstOrDefault();
                return result;
            } 
        }

        public bool Update(int id,Employee emp)
        {
            using(var context=new EmployeeEntities())
            {
                var result = context.Employee_Master.FirstOrDefault(x => x.EmpId == id);
                if(result!=null)
                 {
                    result.Name = emp.Name;
                    result.Date_Of_Birth = emp.Date_Of_Birth;
                    result.Phone = emp.Phone;
                    result.Address = emp.Address;
                    result.Country = emp.Country;
                    result.State = emp.State;
                    result.City = emp.City;
                    result.EmailId = emp.EmailId;
                    result.IsActive = true;
                    result.CreatedbY = Guid.NewGuid();
                    result.Createdon = DateTime.UtcNow;
                    result.ModifiedBy = Guid.NewGuid();
                    result.ModiefiedOn = DateTime.UtcNow;
                 }
                context.Entry(result).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            using(var context=new EmployeeEntities())
            {
                var result = context.Employee_Master.FirstOrDefault(x => x.EmpId == id);
                context.Entry(result).State = EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
        }
    }
}