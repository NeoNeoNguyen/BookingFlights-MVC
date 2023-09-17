using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BookingAirline.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        // GET: Email
        public ActionResult SendEmail(string useremail)
        {
            string subject = "Đây là tin nhắn tự động từ hệ thống POS";
            string body = "Xin chào bạn, bạn vừa đặng nhập vào trang web tội phạm , Nếu không phải là bạn vui lòng cung cấp thông tin của bạn tại https://luatduonggia.vn/cach-lam-don-gui-don-to-cao-online-qua-mang-moi-nhat/. !";
            WebMail.Send(useremail, subject, body, null, null, null, true, null, null, null, null, null, null);
            ViewBag.msg = "Email đã được gửi thành công ...";
            return View();
        }


    }
}
