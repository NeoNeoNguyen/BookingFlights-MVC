using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingAirline.Models;

namespace BookingAirline.Controllers
{
    public class LoginAdminController : Controller
    {
        BookingAirLightEntities database = new BookingAirLightEntities();
        // GET: LoginAdmin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            var data = database.NhanViens.Where(s => s.UserName == user && s.Password == password).FirstOrDefault();
            var taikhoan = database.NhanViens.SingleOrDefault(s => s.UserName == user && s.Password == password);
            if (data == null)
            {
                TempData["error"] = "Tài khoản đăng nhập không đúng";
                return View();
            }
            else
            {
                Session["userNV"] = data;
                return RedirectToAction("TrangChu", "NhanVien");
            }
            ///*else if (taikhoan != null)
            //{
            //    //add session
            //    database.Configuration.ValidateOnSaveEnabled = false;
                
            //}*/
        }


        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "LoginAdmin");
        }

    }
}
