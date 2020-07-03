using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tech_Admin.Models.DbOperation;
using Tech_Admin.Models;


namespace Tech_Admin.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        EmployeeEntities emp = new EmployeeEntities();
        DbOperation db = null;
        public HomeController()
        {
            db = new DbOperation();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Employee model)
        {
            using (var context = new EmployeeEntities())
            {

                bool isValid = context.Employee_Master.Any(x => x.Name == model.Name && x.Password == model.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    return Json(isValid);
                    // return View("Index", "Employee_Master");
                }
            }
            ModelState.AddModelError("", "Invalid Username and Password");
            return Json("error");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Role()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Role(RoleM roleM)
        {
            if (ModelState != null)
            {
                int id = db.RoleEmp(roleM);
                if (id > 0)
                {
                    ModelState.Clear();
                    return RedirectToAction("Index2", "Employee_Master");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }

        [HttpPost]
        public ActionResult Sign_Up()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Phone()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Phonenumber(Phone_Master ph)
        {
            emp.Phone_Master.Add(ph);
            emp.SaveChanges();
            return View();
        }


    }
}