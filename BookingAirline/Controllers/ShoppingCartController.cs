using BookingAirline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingAirline.Controllers
{
    //public class ShoppingCartController : Controller
    //{
    //    BookingAirLightEntities database = new BookingAirLightEntities();
    //    // GET: ShoppingCart
    //    public ActionResult ShowCart()
    //    {
    //        if (Session["Cart"] == null)
    //            return RedirectToAction("ShowCart", "ShoppingCart");
    //        Cart _cart = Session["Cart"] as Cart;
    //        return View(_cart);
    //    }

    //    //Them ve vao gio hang
    //    //Action tao moi gio hang
    //    public Cart GetCart()
    //    {
    //        Cart cart = Session["Cart"] as Cart;
    //        if (cart == null || Session["Cart"] == null)
    //        {
    //            cart = new Cart();
    //            Session["Cart"] = cart;
    //        }
    //        return cart;
    //    }

    //    //Action them product vao gio hang
    //    public ActionResult AddToCart(string id)
    //    {
    //        var _pro = database.ChuyenBays.SingleOrDefault(s => s.MaCB == id); //Lay pro theo MaCB
    //        if (_pro == null)
    //        {
    //            GetCart().Add_Product_Cart(_pro);
    //        }
    //        return RedirectToAction("ShowCart", "ShoppingCart");
    //    }


    //    //Action cap nhat so luong gio hang
    //    public ActionResult Update_Cart_Quantity(FormCollection form)
    //    {
    //        Cart cart = Session["Cart"] as Cart;
    //        string id_pro = form["IDTicket"];
    //        int _quantity = int.Parse(form["CartQuantity"]);
    //        cart.Update_quantity(id_pro, _quantity);
    //        return RedirectToAction("ShowCart", "ShoppingCart");
    //    }

    //    //Action xoa ve trong gio hang
    //    public ActionResult RemoveCart(string id)
    //    {
    //        Cart cart = Session["Cart"] as Cart;
    //        cart.Remove_CartItem(id);
    //        return RedirectToAction("ShowCart", "ShoppingCart");
    //    }

    //    //Hien thi so luong san pham len gio hang tren trang index
    //    public PartialViewResult BagCart()
    //    {
    //        int total_quantity_item = 0;
    //        Cart cart = Session["Cart"] as Cart;
    //        if (cart != null)
    //        {
    //            total_quantity_item = cart.Total_quantity();
    //        }
    //        ViewBag.QuantityCart = total_quantity_item;
    //        return PartialView("BagCart");
    //    }
    //}
}