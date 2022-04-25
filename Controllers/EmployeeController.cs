using AdminPanelCoreNitinChandra.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NitinChandraSecondTest.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NitinChandraSecondTest.Controllers
{
    public class EmployeeController : Controller
    {
        NitinChandraContext Db = new NitinChandraContext();
        public IActionResult Index()
        {
            return View();
        }
         public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Ragistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ragistration(UserDetail Rg)
        {
            UserDetail newemp = new UserDetail();
            newemp.Name = Rg.Name;
            newemp.Email = Rg.Email;
            newemp.Address = Rg.Address;
            newemp.Age = Rg.Age;
            newemp.Password = Rg.Password;

            Db.UserDetails.Add(newemp);
            Db.SaveChanges();

            return RedirectToAction("Table");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserDetail logn)
        {
            var loginname = Db.UserDetails.Where(a => a.Name == logn.Name).FirstOrDefault();
            var loginemail = Db.UserDetails.Where(a => a.Email == logn.Email).FirstOrDefault();
            var loginps = Db.UserDetails.Where(a => a.Password == logn.Password).FirstOrDefault();
            if (loginname == null && loginemail == null && loginps == null)
            {
                TempData["name1"] = "Your Name, Email And Password is Wrong";
            }
            else if (loginemail == null && loginps == null)
            {
                TempData["email2"] = "Your Email And Password is Wrong";
            }
            else if (loginemail == null)
            {
                TempData["em"] = "Your Email is Wrong";
            }
            else if (loginps == null)
            {
                TempData["pw"] = "Your Password is Wrong";
            }
            else if (loginname.Name == logn.Name && loginemail.Email == logn.Email && loginps.Password == logn.Password)
            {
                var claims = new[] {new Claim(ClaimTypes.Name, loginname.Name),
                                    new Claim(ClaimTypes.Email, loginname.Email)};
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);
                HttpContext.Session.SetString("Name1", logn.Name);
                return RedirectToAction("Table");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult Table()
        {
            var data = Db.Employees.ToList();

            List<EmployeeModel> empmodel = new List<EmployeeModel>();

            foreach (var item in data)
            {
                empmodel.Add(new EmployeeModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Address = item.Address,
                    Age = item.Age,
                    Course = item.Course
                });
            }
            return View(empmodel);
        }

        public IActionResult AddDetails()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDetails(EmployeeModel e)
        {
            Employee maintable = new Employee();
            maintable.Id = e.Id;
            maintable.Name = e.Name;
            maintable.Email = e.Email;
            maintable.Address = e.Address;
            maintable.Age = e.Age;
            maintable.Course = e.Course;

            if (e.Id == 0)
            {
                Db.Employees.Add(maintable);
                Db.SaveChanges();
            }
            else
            {
                Db.Entry(maintable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Db.SaveChanges();
            }
            
            return RedirectToAction("Table");
        }

        public IActionResult Delete(int id)
        {
            var deleteitem = Db.Employees.Where(model => model.Id == id).First();
            Db.Employees.Remove(deleteitem);
            Db.SaveChanges();
            return RedirectToAction("Table");
        }
        public IActionResult Edit(int id)
        {
            EmployeeModel dbedit = new EmployeeModel();
            var editetem = Db.Employees.Where(model => model.Id == id).First();
            dbedit.Id = editetem.Id;
            dbedit.Name = editetem.Name;
            dbedit.Email = editetem.Email;
            dbedit.Address = editetem.Address;
            dbedit.Age = editetem.Age;
            dbedit.Course = editetem.Course;
            
            return View("AddDetails", dbedit);
        }

    }
}
