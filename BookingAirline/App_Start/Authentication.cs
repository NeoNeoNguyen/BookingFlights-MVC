using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BookingAirline.Models;

namespace BookingAirline.App_Start
{
    public class Authentication: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1.Check session  : Đã đăng nhập vào hệ thống = > cho thực hiện filterContext
            //Ngược lại thì cho trở lại trang đăng nhập 
            KhachHang nvSession = (KhachHang)HttpContext.Current.Session["userKH"];
            if (nvSession == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "LoginUser", action = "Login" }));
            }
            return;

        }
    }
}