using BookingAirline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookingAirline.App_Start
{
    public class AuthenticationNV : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1.Check session  : Đã đăng nhập vào hệ thống = > cho thực hiện filterContext
            //Ngược lại thì cho trở lại trang đăng nhập 
            NhanVien nvSession = (NhanVien)HttpContext.Current.Session["userNV"];
            if (nvSession == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "LoginAdmin", action = "Login" }));
            }
            return;

        }
    }
}