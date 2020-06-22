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

namespace Tech_Admin.Controllers
{
    public class Employee_MasterController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();
        private DbOperation db2 = new DbOperation();

        // GET: Employee_Master
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            var employee_Master = db.Employee_Master.Where(x => x.IsActive == true).Include(e => e.City_Info).Include(e => e.Country_Info).Include(e => e.RoleMaster).Include(e => e.State_info).ToList();
            return View(employee_Master);
        }


        [Authorize(Roles = "Admin,User")]
        public JsonResult Index1()
        {
            //var employee_Master = db.Employee_Master.Where(x => x.IsActive == true).Include(e => e.City_Info).Include(e => e.Country_Info).Include(e => e.RoleMaster).Include(e => e.State_info).ToList();

            var employee_Master = db2.Getrecord();
            return Json(new { data = employee_Master }, JsonRequestBehavior.AllowGet);
        }



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

        // POST: Employee_Master/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
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



    }
}
