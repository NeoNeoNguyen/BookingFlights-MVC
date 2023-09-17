using BookingAirline.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BookingAirline.Controllers
{
    public class KhachHangController : Controller
    {
        BookingAirLightEntities database = new BookingAirLightEntities();
        //GET: KhachHang
        public ActionResult TrangChu()
        {
            var check = Session["userKH"];
            if (check != null)
            {
                return RedirectToAction("TrangChu", "KhachHangHA");
            }
            return View();
        }
        public ActionResult DSachCB()
        {
            //Lấy số lượng chỗ ngồi
            var soluong = Int64.Parse(Request["aldults"]) + Int64.Parse(Request["children"]);
            Session["SoLuong"] = soluong;
            Session["From"] = Request["from"];
            Session["To"] = Request["to"];
            Session["Trip"] = Request["trip"];
            Session["Return"] = Request["return"];
            DateTime ngaykh = Convert.ToDateTime(Request["deparure"]);
            var month = ngaykh.ToString("MM");
            var Day = ngaykh.ToString("dd");
            var year = ngaykh.ToString("yyyy");
            //Lọc tìm kiếm chuyến bay
            var di = Request["From"].ToString();
            var den = Request["to"].ToString();
            var chuyendi = database.TuyenBays.Where(s => s.SanBayDi == di && s.SanBayDen == den).FirstOrDefault();
            //var listdi = database.ChuyenBays.Where(s => s.MaTBay == chuyendi.MaTBay && Convert.ToDateTime(s.NgayGio).ToString("dd")== Day ).ToList();
            var test = database.ChuyenBays.SqlQuery
                ("Select * from ChuyenBay where YEAR(NgayGio)= @year and DAY (NgayGio) = @day and MONTH(NgayGio)= @month and MaTbay=@chuyendi",
                new SqlParameter("@year", year),
                new SqlParameter("@day", Day),
                new SqlParameter("@month", month),
                new SqlParameter("@chuyendi", chuyendi.MaTBay)
                ).ToList();

            //Hiển thị danh sách các chuyến bay
            return View(test);
        }

        public ActionResult DSachCBVe(string id)
        {
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            OrderStatu order = new OrderStatu();
            order.IDUser = uid;
            var count = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            if (count != null)
            {
                database.OrderStatus.Remove(count);
                database.SaveChanges();
            }
            //Kiểm tra nếu là khứ hồi
            if (Session["trip"] == null)
            {
                return RedirectToAction("TrangChu");
            }
            var check = Session["trip"].ToString();
            if (check == "one-way")
            {
                order.MaCBdi = id;
                database.OrderStatus.Add(order);
                database.SaveChanges();
                return RedirectToAction("ChooseSeat");
            }
            else
            {
                order.MaCBdi = id;
                database.OrderStatus.Add(order);
                database.SaveChanges();

                DateTime ngaykh = Convert.ToDateTime(Session["Return"]);
                var month = ngaykh.ToString("MM");
                var Day = ngaykh.ToString("dd");
                var year = ngaykh.ToString("yyyy");

                ///Kiểm tra và xuất dữ liệu vé theo trước
                var di = Session["To"].ToString();
                var den = Session["From"].ToString();
                var chuyenve = database.TuyenBays.Where(s => s.SanBayDi == di && s.SanBayDen == den).FirstOrDefault();
                //var listcv = database.ChuyenBays.Where(s => s.MaTBay == chuyenve.MaTBay).ToList();
                var test = database.ChuyenBays.SqlQuery
                    ("Select * from ChuyenBay where YEAR(NgayGio)= @year and DAY (NgayGio) = @day and MONTH(NgayGio)= @month and MaTBay = @chuyenve",
                        new SqlParameter("@year", year),
                        new SqlParameter("@day", Day),
                        new SqlParameter("@month", month),
                        new SqlParameter("@chuyenve", chuyenve.MaTBay)
                        ).ToList();
                return View(test);
            }

        }
        [HttpGet]
        public ActionResult DSachCBVe01(string id)
        {
            var check = Session["trip"].ToString();
            if (check == "round")
            {
                var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                OrderStatu order = new OrderStatu();
                order = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
                order.MaCBve = id;
                database.Entry(order).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                Session["Mave"] = id;
                return RedirectToAction("ChooseSeat","KhachHang");
            }
            return View();
        }

        public ActionResult DienThongTinKH(string id)
        {
            Session["SLKH"] = null;
            Cart cart = Session["Cart"] as Cart;
            var check = Session["trip"].ToString();
            if (check == "round")
            {
                Session["SLKH"] = (cart.Items.Count() / 2);
            }
            else if (check == "one-way")
            {
                Session["SLKH"] = cart.Items.Count();
            }
            return View(cart);
        }

        [HttpPost]
        public ActionResult DienThongTinKH()
        {
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dsorder = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            //Tạo mới danh sách các khách hàng mới tham gia vào hệ thống
            KhachHang kh = new KhachHang();
            Random rd = new Random();
            //Lấy thông tin khách hàng khi có nhiều vé
            var di = Session["From"].ToString();
            var den = Session["To"].ToString();
            var checkkhuhoi = Session["Return"];
            Cart cart = Session["Cart"] as Cart;
            var stt = 0;
            if (checkkhuhoi == null)
            {
                foreach (var item01 in cart.Items)
                {
                    
                    var mave1 = item01.idVe.MaVe;
                    var cccd = Request["cccd_" + stt];
                    var TenKh = Request["TenKH_" + stt];
                    var Ngaysinh = Request["NgaySinh_"+stt];
                    var Email = Request["Email_" + stt];
                    var Phone = Request["Phone_" + stt];
                    var Gioitinh = Request["GioiTinh_" + stt];
                    
                    
                    //Kiểm tra và thêm các thông tin khách hàng vào danh sách khách hàng tham gia hệ thống
                    var check01 = database.KhachHangs.Where(s => s.CCCD == cccd).FirstOrDefault();
                    //Kiểm tra nếu mã cccd này chưa có thông tin sẽ lưu lại
                    if(check01 == null)
                    {
                        kh = new KhachHang();
                        kh.IDKH = "KH"+ rd.Next(0, 10000); // Hàm random thông tin khách hàng kèm 3 số cuối theo mã cccd
                        kh.CCCD = cccd;
                        kh.TenKH = TenKh;
                        kh.SDT = Phone;
                        kh.Email = Email;
                        kh.GioiTinh = Gioitinh;
                        kh.NgaySinh = Ngaysinh;
                        database.KhachHangs.Add(kh);
                        database.SaveChanges();
                        cart.CapNhatCCCD(mave1, cccd, TenKh, Ngaysinh, Email, Phone, Gioitinh, kh.IDKH);
                    }
                    else
                    {
                        cart.CapNhatCCCD(mave1, cccd, TenKh, Ngaysinh, Email, Phone, Gioitinh, check01.IDKH);
                    }
                    
                    stt++;
                }
            }
            else
            {
                var cbdi = dsorder.MaCBdi;
                var cbden = dsorder.MaCBve;
                var number = 0;
                var number2 = 0;
                foreach (var item01 in cart.Items)
                {

                    //Chép cccd vào vé lúc đi của khách khàng
                    if (item01.idVe.MaCB == cbdi)
                    {
                        var mave1 = item01.idVe.MaVe;
                        var cccd = Request["cccd_" + number];
                        var TenKh = Request["TenKH_" + number];
                        var Ngaysinh = Request["NgaySinh_" + number];
                        var Email = Request["Email_" + number];
                        var Phone = Request["Phone_" + number];
                        var Gioitinh = Request["GioiTinh_" + number];
                        
                        //Kiểm tra và thêm các thông tin khách hàng vào danh sách khách hàng tham gia hệ thống
                        var check01 = database.KhachHangs.Where(s => s.CCCD == cccd).FirstOrDefault();
                        //Kiểm tra nếu mã cccd này chưa có thông tin sẽ lưu lại
                        if (check01 == null)
                        {
                            kh = new KhachHang();
                            kh.IDKH = "KH" + rd.Next(0, 10000); // Hàm random thông tin khách hàng kèm 3 số cuối theo mã cccd
                            kh.CCCD = cccd;
                            kh.TenKH = TenKh;
                            kh.SDT = Phone;
                            kh.Email = Email;
                            kh.GioiTinh = Gioitinh;
                            kh.NgaySinh = Ngaysinh;
                            database.KhachHangs.Add(kh);
                            database.SaveChanges();
                            cart.CapNhatCCCD(mave1, cccd, TenKh, Ngaysinh, Email, Phone, Gioitinh, kh.IDKH);
                        }
                        else
                        {
                            cart.CapNhatCCCD(mave1, cccd, TenKh, Ngaysinh, Email, Phone, Gioitinh, check01.IDKH);
                        }     
                        number++;
                    }
                    //Chép cccd vào vè lúc về của khách hàng
                    else if (item01.idVe.MaCB == cbden)
                    {

                        var mave2 = item01.idVe.MaVe;
                        var cccd2 = Request["cccd_" + number2];
                        var TenKh = Request["TenKH_" + number2];
                        var Ngaysinh = Request["NgaySinh_" + number2];
                        var Email = Request["Email_" + number2];
                        var Phone = Request["Phone_" + number2];
                        var Gioitinh = Request["GioiTinh_" + number2];
                        
                        //Kiểm tra và thêm các thông tin khách hàng vào danh sách khách hàng tham gia hệ thống
                        var check01 = database.KhachHangs.Where(s => s.CCCD == cccd2).FirstOrDefault();
                        //Kiểm tra nếu mã cccd này chưa có thông tin sẽ lưu lại
                        if (check01 == null)
                        {
                            kh = new KhachHang();
                            kh.IDKH = "KH" + rd.Next(0, 10000); // Hàm random thông tin khách hàng kèm 3 số cuối theo mã cccd
                            kh.CCCD = cccd2;
                            kh.TenKH = TenKh;
                            kh.SDT = Phone;
                            kh.Email = Email;
                            kh.GioiTinh = Gioitinh;
                            kh.NgaySinh = Ngaysinh;
                            database.KhachHangs.Add(kh);
                            database.SaveChanges();
                            cart.CapNhatCCCD(mave2, cccd2, TenKh, Ngaysinh, Email, Phone, Gioitinh, kh.IDKH);
                        }
                        else
                        {
                            cart.CapNhatCCCD(mave2, cccd2, TenKh, Ngaysinh, Email, Phone, Gioitinh, check01.IDKH);
                        }
                        number2++;
                    }
                    
                }
            }

            var total = 0;
            #region Bancu
            //var mave = "VE" + rd.Next(1, 1000);
            //check ma ve duoi database
            //Tao ve moi
            //Ve ve = new Ve();
            //ve.MaVe = mave;
            //ve.MaCB = dsorder.MaCBdi;
            //ve.TinhTrang = "Chưa thanh toán";
            //ve.GiaVe = 500000;
            //ve.CCCD = Request["cccd"];
            //ve.IDKH = "Vang Lai";
            //ve.MaHV = "HV01";
            //total +=(int) ve.GiaVe;
            //database.Ves.Add(ve);
            //database.SaveChanges();
            //dsorder.MaCBdi = ve.MaVe;
            //database.Entry(dsorder).State = System.Data.Entity.EntityState.Modified;
            //database.SaveChanges();
            ////Kiểm tra nếu có khứ hồi 
            //if (dsorder.MaCBve != null)
            //{
            //    ve = new Ve();
            //    mave = "VE" + rd.Next(1, 1000);
            //    ve.MaVe = mave;
            //    ve.MaCB = dsorder.MaCBve;
            //    ve.TinhTrang = "Chưa thanh toán";
            //    ve.GiaVe = 500000;
            //    ve.CCCD = Request["cccd"];
            //    ve.IDKH = "Vang Lai";
            //    ve.MaHV = "HV01";
            //    total += (int)ve.GiaVe;
            //    database.Ves.Add(ve);
            //    database.SaveChanges();
            //    dsorder.MaCBve = ve.MaVe;
            //    database.Entry(dsorder).State = System.Data.Entity.EntityState.Modified;
            //    database.SaveChanges();
            //}
            #endregion
            var contact = new Order();
            contact.CreateDate = DateTime.Now;
            contact.ShipName = Request["TenKH_0"];
            contact.ShipEmail = Request["Email_0"];
            contact.NumberPhone = Request["Phone_0"];
            contact.CCCD = Request["cccd_0"];
            contact.Total = total;
            Session["contacKH"] = contact;
            return RedirectToAction("ThanhToan");
        }
        //Version 2.0
        //Chọn chỗ ngồi lúc đi
        [HttpGet]
        public ActionResult ChooseSeat()
        {
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dsorder = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            var dsve = database.Ves.Where(s => s.MaCB == dsorder.MaCBdi).ToList();
            ViewData["MaCB"] = dsorder.MaCBdi;
            return View(dsve);
        }
        [HttpPost]
        public ActionResult ChooseSeat(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                cart.XoaSauKhiDat();
            }
            int check = 0;
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dsorder = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            var dsve = database.Ves.Where(s => s.MaCB == dsorder.MaCBdi).ToList();
            for (int i = 1; i <= dsve.Count(); i++)
            {
                if (Request["Ma" + i] != null)
                {
                    //14/03/2023 Suy nghĩ làm thêm luồn xử lý dữ liệu
                    //Add thông tin vé vào giỏ hàng
                    var ticket = Request["Ma" + i];
                    var detailtic = database.Ves.Where(s => s.MaCB == dsorder.MaCBdi && s.MaVe == ticket).FirstOrDefault();
                    GetCart().Add(detailtic,1,null);                 
                    check++;
                    
                }
                if (check == id)
                {
                    break;
                }

            }
            var ktkh = Session["trip"].ToString();
            if (ktkh == "round")
            {
                return RedirectToAction("ChooseSeatVe");
            }
            return RedirectToAction("DienThongTinKH");
        }

        //Chọn chỗ ngồi lúc về
        [HttpGet]
        public ActionResult ChooseSeatVe()
        {
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dsorder = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            var dsve = database.Ves.Where(s => s.MaCB == dsorder.MaCBve).ToList();
            ViewData["MaCB"] = dsorder.MaCBve;
            return View(dsve);
        }
        [HttpPost]
        public ActionResult ChooseSeatVe(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            int check = 0;
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var dsorder = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            var dsve = database.Ves.Where(s => s.MaCB == dsorder.MaCBve).ToList();
            for (int i = 1; i <= dsve.Count(); i++)
            {
                if (Request["Ma" + i] != null)
                {
                    //Add thông tin vé vào giỏ hàng
                    var ticket = Request["Ma" + i];
                    var detailtic = database.Ves.Where(s => s.MaCB == dsorder.MaCBve && s.MaVe == ticket).FirstOrDefault();
                    GetCart().Add(detailtic, 1,null);
                    check++;
                    
                }
                if (check == id)
                {
                    break;
                }

            }
            return RedirectToAction("DienThongTinKH");
        }
        // Tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {
            if (Session["Cart"] == null)
            {
                return View();
            }
            Cart cart = Session["Cart"] as Cart;
            var tt = cart.TongTien();
            var contact = (Order)Session["contacKH"];
            contact.Total = tt;
            Session["contacKH"] = contact;
            return View(cart);
        }
        public ActionResult ConfirmTT(string id)
        {
            var uid = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            var kh = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();

            var maveve = database.Ves.Where(s => s.MaCB == kh.MaCBve).FirstOrDefault();
            var ttkh = (Order)Session["contacKH"]; // Thông tin liên lạc của KH
            var tongtien = string.Format("{0:0,0 vnđ}", ttkh.Total);
            //mavedi.TinhTrang = "Đã thanh toán";
            //database.Entry(mavedi).State = System.Data.Entity.EntityState.Modified;
            //database.SaveChanges();
            //Thêm lưu xuất ra hóa đơn
            var idkh = database.KhachHangs.Where(s => s.CCCD == ttkh.CCCD).FirstOrDefault();
            Random rd = new Random();
            HoaDon themhd = new HoaDon();
            themhd.MaHD = (string)Session["madonhang"];
            themhd.TinhTrang = "Đã thanh toán";
            themhd.NgayLap = System.DateTime.Now;
            themhd.ThanhTien = ttkh.Total;
            themhd.IDKH = idkh.IDKH;
            themhd.CCCD = ttkh.CCCD;
            database.HoaDons.Add(themhd);
            database.SaveChanges();
            //Thêm chi tiết hóa đơn
            ChiTietHD cthd = new ChiTietHD();
            cthd.MaHD = themhd.MaHD;

            Cart cart = Session["Cart"] as Cart;
            var dsorder = cart.Items;
            foreach (var item in dsorder)
            {
                var mavedi = database.Ves.Where(s => s.MaVe == item.idVe.MaVe && s.MaCB == item.idVe.MaCB).FirstOrDefault();
                mavedi.TinhTrang = "Đã thanh toán";
                mavedi.IDKH = item.IDKH;
                mavedi.CCCD = item.CCCD;
                database.Entry(mavedi).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();

                cthd.MaVe = item.idVe.MaVe;
                cthd.SoLuong = item.soLuong;
                cthd.DonGia = item.idVe.GiaVe;
                cthd.MaCB = item.idVe.MaCB;
                cthd.TongTien = (item.soLuong) * (item.idVe.GiaVe);
                database.ChiTietHDs.Add(cthd);
                database.SaveChanges();

                //Tiến hành thiết lập tạo phiếu đặt chỗ cho khách hàng
                PhieuDatCho pdc = new PhieuDatCho();
                pdc.MaPhieu = "PDC" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                pdc.MaCB = item.idVe.MaCB;
                pdc.IDKH = item.IDKH;
                pdc.CCCD = item.CCCD;
                pdc.NgayDat = System.DateTime.Now;
                pdc.SoGhe = item.idVe.MaVe;
                database.PhieuDatChoes.Add(pdc);
                database.SaveChanges();
            }
            //if (maveve != null)
            //{

            //    maveve.TinhTrang = "Đã thanh toán";
            //    database.Entry(maveve).State = System.Data.Entity.EntityState.Modified;
            //    database.SaveChanges();
            //};
            //Render form gửi email về cho khách hàng
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/Template/HtmlPage1.html"));
            content = content.Replace("{{CustomerName}}", ttkh.ShipName);
            content = content.Replace("{{Phone}}", ttkh.NumberPhone);
            content = content.Replace("{{Email}}", ttkh.ShipEmail);
            content = content.Replace("{{Total}}", tongtien);
            content = content.Replace("{{Thoigian}}", Convert.ToString(ttkh.CreateDate));
            content = content.Replace("{{Invoice}}", themhd.MaHD);
            string subject = "Đây là tin nhắn tự động từ hệ thống POS";
            WebMail.Send(ttkh.ShipEmail, subject, content, null, null, null, true, null, null, null, null, null, null);
            cart.XoaSauKhiDat();
            var count = database.OrderStatus.Where(s => s.IDUser == uid).FirstOrDefault();
            database.OrderStatus.Remove(count);
            database.SaveChanges();
            return RedirectToAction("ThankYou");
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        public ActionResult DSChuyenBayTest()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult WhishList(string id)
        {
            var KH = (BookingAirline.Models.KhachHang)Session["userKH"];
            //Xác thực người dùng đã đăng nhập hay chưa
            if (Session["userKH"] != null)
            {
                var faCB = database.Wishlists.Where(s => s.MaCB == id && s.MaKH == KH.IDKH).FirstOrDefault();
                if (faCB == null)
                {
                    Wishlist wl = new Wishlist();
                    Random rd = new Random();
                    var rdnumber = rd.Next(1, 1000);
                    wl.MaWL = rdnumber.ToString();
                    wl.MaCB = id;
                    wl.MaKH = KH.IDKH;
                    wl.NgayThem = System.DateTime.Now;
                    database.Wishlists.Add(wl);
                    database.SaveChanges();
                }
            }
            else
            {
                return RedirectToAction("Login", "LoginUser");
            }
            return RedirectToAction("ChooseSeat", "KhachHangHA");
        }
        public ActionResult FailePayment()
        {
            return View();
        }
    }
}
