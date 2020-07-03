using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Tech_Admin.Models;
using Tech_Admin.Models.DbOperation;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Configuration;
using System.Data.SqlClient;
using Tech_Admin.Connection_String;
using WordToPDF;
using Newtonsoft.Json;
using System.Web.UI;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace Tech_Admin.Controllers
{
    public class Employee_MasterController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();
        private DbOperation db2 = new DbOperation();
        private DAL db3 = new DAL();
        // GET: Employee_Master
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            var employee_Master = db.Employee_Master.Where(x => x.IsActive == true).Include(e => e.City_Info).Include(e => e.Country_Info).Include(e => e.RoleMaster).Include(e => e.State_info).ToList();
            return View(employee_Master);
        }
       
        #region JqGrid
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Getdata()
        {
            
            List<Employee_Master> emplist = new List<Employee_Master>();
            emplist = db3.GetData();
            List<Employee_Master> emp2 = new List<Employee_Master>();
            for (int i = 0; i < emplist.Count; i++)
            {
                Employee_Master emp3 = new Employee_Master();
                emp3 = emplist[i];
                emp2.Add(emp3);

            }

            // var emp2 = db2.GetRecord();
            return Json( emp2 , JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Create User
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Country = new SelectList(db.Country_Info, "Country_Id", "Country_name");

            ViewBag.Country1 = db.Country_Info.ToList();
            ViewBag.RoleId1 = db.RoleMaster.ToList();
            ViewBag.RoleId = new SelectList(db.RoleMaster, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Employee_Master emp3)
        {

            emp3.IsActive = true;
            emp3.CreatedbY = Guid.NewGuid();
            emp3.Createdon = DateTime.Now;

            string emailmsg = "Your Email Id" + emp3.EmailId + ", is being registered with us" +
                "Kindly Check Your Credential For Login <br/> User Name:" + emp3.Name + "Password:" + emp3.Password + "<br/> From:"
                + Resource1.Email_Account + "<br/> Phone No: 900324931";
            string emailSubject = Resource1.Email_Subject + "Login Credential";



            db.Employee_Master.Add(emp3);
            db.SaveChanges();
            await this.SendEmail(emp3.EmailId, emailmsg, emailSubject);
            return Json(emp3.EmpId);
        }

        #endregion 

        #region sendmail
        public async Task<bool> SendEmail(string email, string msg, string subject = "")
        {
            bool isSend = false;
            try
            {
                var body = msg;
                var message = new MailMessage();

                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(Resource1.Email_Account);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : Resource1.Email_Subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = Resource1.Email_Account,
                        Password = Resource1.Email_Password
                    };
                    smtp.Credentials = credential;
                    smtp.Host = Resource1.Smtp_Gmail;
                    smtp.Port = Convert.ToInt32(Resource1.Smtp_Port);
                    smtp.EnableSsl = true;


                    await smtp.SendMailAsync(message);

                    isSend = true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return isSend;
        }
        #endregion


        #region Dropdownlist
        public JsonResult GetState(int Cid)
        {
            List<State_info> Statelist = db.State_info.Where(x => x.Country_RefId == Cid).ToList();
            var stateList = Statelist.Select(m => new SelectListItem()
            {
                Text = m.State_Name,
                Value = m.State_Id.ToString(),
            });
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCity(int Sid)
        {
            List<City_Info> Citylist = db.City_Info.Where(x => x.State_RefId == Sid).ToList();
            var cityList = Citylist.Select(m => new SelectListItem()
            {
                Text = m.City_Name,
                Value = m.City_Id.ToString(),
            });
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region EditCredentials
        // GET: Employee_Master/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Master employee_Master = db.Employee_Master.Find(id);
            if (employee_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.City = new SelectList(db.City_Info, "City_Id", "City_Name", employee_Master.City);
            ViewBag.Country = new SelectList(db.Country_Info, "Country_Id", "Country_name", employee_Master.Country);
            ViewBag.RoleId = new SelectList(db.RoleMaster, "RoleId", "RoleName", employee_Master.RoleId);
            ViewBag.State = new SelectList(db.State_info, "State_Id", "State_Name", employee_Master.State);

            //ViewBag.Country1 = db.Country_Info.ToList();
            //ViewBag.State1 = db.State_info.ToList();
            //ViewBag.City1 = db.City_Info.ToList();
            ViewBag.RoleId1 = db.RoleMaster.Where(x => x.isActive == true).ToList();


            return View(employee_Master);
        }



        // POST: Employee_Master/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Employee_Master employee_Master)
        {
            if (ModelState.IsValid)
            {
                employee_Master.ModiefiedOn = DateTime.Now;
                employee_Master.ModifiedBy = Guid.NewGuid();
                employee_Master.IsActive = true;
                db.Entry(employee_Master).State = EntityState.Modified;
                db.SaveChanges();
                return Json("success");
            }
            ViewBag.City = new SelectList(db.City_Info, "City_Id", "City_Name", employee_Master.City);
            ViewBag.Country = new SelectList(db.Country_Info, "Country_Id", "Country_name", employee_Master.Country);
            ViewBag.RoleId = new SelectList(db.RoleMaster, "RoleId", "RoleName", employee_Master.RoleId);
            ViewBag.State = new SelectList(db.State_info, "State_Id", "State_Name", employee_Master.State);
            ViewBag.RoleId1 = db.RoleMaster.Where(x => x.isActive == true).ToList();
            return Json("Error");
        }


        #endregion


        #region Delete
        // GET: Employee_Master/Delete/5
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Master employee_Master = db.Employee_Master.Find(id);
            if (employee_Master == null)
            {
                return HttpNotFound();
            }
            return View(employee_Master);
        }

        // POST: Employee_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee_Master employee_Master = db.Employee_Master.Find(id);
            db.Employee_Master.Remove(employee_Master);
            db.SaveChanges();
            return Json(employee_Master);
        }
        #endregion

        #region MiscOperation





        // GET: Employee_Master/Details/5
        [Authorize(Roles = "Admin,User")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee_Master employee_Master = db.Employee_Master.Find(id);
            if (employee_Master == null)
            {
                return HttpNotFound();
            }
            return View(employee_Master);
        }
        #endregion


        #region Check Name EmailID Phone Availability
        public JsonResult Nameavailability(string name)
        {
            bool result = !db.Employee_Master.ToList().Exists(model => model.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            //var result = db.Employee_Master.Where(x => x.Name == name).Where(x => x.IsActive == true).FirstOrDefault();
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Emailavailability(string emailId)
        {
            bool result = !db.Employee_Master.ToList().Exists(model => model.EmailId.Equals(emailId, StringComparison.CurrentCultureIgnoreCase));
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult Phoneavailability(string phone)
        {
            bool result = !db.Employee_Master.ToList().Exists(model => model.Phone.Equals(phone, StringComparison.CurrentCultureIgnoreCase));
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        [Authorize (Roles="Admin,User")]
        #region File Upload
        public ActionResult FileUpload()
        {
            return View();
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult File_Upload()
        {
            //Word2Pdf objWord = new Word2Pdf();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        string fname;
                        fname = file.FileName;
                        
                        string fpath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["Upload"]), fname);
                        file.SaveAs(fpath);

                        int size = file.ContentLength;
                        string ftype = file.ContentType;
                        Guid Uid = Guid.NewGuid();
                        Guid create = Guid.NewGuid();
                        Guid user = Guid.NewGuid();
                        bool valid = true;
                        string createon = DateTime.Now.ToString();
                        bool upload = Uploadfile(Uid, fname, create, createon, ftype, size, fpath, user, valid);
                    }
                }
                return Json("Success");
            }
            catch (Exception e)
            {
                return Json("Error" + e.Message);
            }
        }

        public bool Uploadfile(Guid uploadid,string Fname,Guid createby,string createon,string ftype,int fsize,string fpath,Guid userrefid,bool isvalid)
        {
            SqlConnection conn = new SqlConnection(AppSettingConnectionString.GetConnectionString("entity"));
            
            SqlCommand cmd = new SqlCommand("spUpload", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            cmd.Parameters.AddWithValue("@UploadId", uploadid);
            cmd.Parameters.AddWithValue("@File_name", Fname);
            cmd.Parameters.AddWithValue("@File_type", ftype);
            
            cmd.Parameters.AddWithValue("@File_size", fsize);
            cmd.Parameters.AddWithValue("@File_path", fpath);
            cmd.Parameters.AddWithValue("@IsActive", isvalid);

            cmd.Parameters.AddWithValue("@CreatedBy", createby);
            cmd.Parameters.AddWithValue("@CreatedOn", createon);
            cmd.Parameters.AddWithValue("@Userid", userrefid);
            cmd.ExecuteScalar();
            //conn.Close();
            return true;
        }


        #endregion



        #region GetCountrylist GetState GetCity
        public ActionResult GetCountry()

        {
            List<Country_Info> countryList = db3.GetCountry();
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetState2()
        {
            List<State_info> stateList = db3.GetState();
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCity2()

        {
            List<City_Info> cityList = db3.GetCity();
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }


        #endregion






        #region download
        public ActionResult ExportToExcel()
        {
            List<Employee_Master> emplist = new List<Employee_Master>();
            emplist = db.Employee_Master.ToList();

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from p in emplist
                              select new
                              {
                                  Name = p.Name.ToString(),
                                  Dob = p.Date_Of_Birth.ToString(),
                                  Country = p.Country_Info.Country_name.ToString(),
                                  City = p.City_Info.City_Name.ToString(),
                                  State=p.State_info.State_Name.ToString(),
                                  EmailId=p.EmailId.ToString(),
                                  Phone=p.Phone.ToString()
                              };
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=MyFile.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            grid.RenderControl(tw);
            Response.Write(sw.ToString());
            Response.End();
            return Json("Success");
        }

        //public ActionResult ExportToExcel1()
        //{
        //    List<Employee_Master> emplist = new List<Employee_Master>();
        //    emplist = db.Employee_Master.ToList();

        //    ExcelPackage pkg = new ExcelPackage();
        //    ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("Reports");

        //    var grid = new System.Web.UI.WebControls.GridView();
        //    grid.DataSource = from p in emplist
        //                      select new
        //                      {
        //                          Name = p.Name.ToString(),
        //                          Dob = p.Date_Of_Birth.ToString(),
        //                          Country = p.Country_Info.Country_name.ToString(),
        //                          City = p.City_Info.City_Name.ToString(),
        //                          State = p.State_info.State_Name.ToString(),
        //                          EmailId = p.EmailId.ToString(),
        //                          Phone = p.Phone.ToString()
        //                      };
        //    grid.DataBind();


        //    ws.Cells["A6"].Value=

        //    return Json("Success");
        //}
        #endregion
    }
}
