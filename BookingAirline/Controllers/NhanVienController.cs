using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingAirline.App_Start;
using BookingAirline.Models;
using Newtonsoft.Json;

namespace BookingAirline.Controllers
{

    [AuthenticationNV]
    public class NhanVienController : Controller
    {
        BookingAirLightEntities database = new BookingAirLightEntities();

        public NhanVien taikhoan()
        {
            NhanVien nv = new NhanVien();
            nv = Session["userNV"] as NhanVien;
            return nv;
        }

        public bool Checkquyen()
        {
            bool thu = false;
            NhanVien check = new NhanVien();
            NhanVien nvSession = taikhoan();

            if( nvSession.MaCV == "NVBV")
            {
                return thu;
            }
            thu = true;
            return thu;
        }
        // GET: NhanVien
        public ActionResult TrangChu()
        {
            return View();

        }

        public ActionResult Scheduleaflight()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSchulea(ChuyenBay cb)
        {
            //Kiểm tra chuyến bay 
            if (cb == null)
            {
                TempData["error"] = "Error";
                return RedirectToAction("Scheduleaflight");
            }
            //Thêm chuyến bay
            else
            {
                Random rd = new Random();
                var macb = "VN" + rd.Next(1, 1000);
                cb.MaCB = macb;
                database.ChuyenBays.Add(cb);
                database.SaveChanges();
                var check = Convert.ToInt32(Request["checkbox01"]);
                if (check == 1)
                {
                    Session["mcb"] = cb.MaCB;
                    TempData["themthongtin"] = check;
                    return RedirectToAction("Scheduleaflight");
                }
                TempData["machuyenbay"] = cb.MaCB;
                TempData["messageAlert"] = "success";
                //Sinh ra vé tự động dựa vào số ghế của chuyến bay
                ThemVe((int)cb.SoLuongGheHang1, (int)cb.SoLuongGheHang2, (int)cb.SoLuongGheHang3, cb);
                return RedirectToAction("Scheduleaflight");
            }

        }
        public void ThemVe(int id1, int id2, int id3, ChuyenBay cb)
        {
            Ve ticket = new Ve();
            //Kiểm tra nếu số lượng
            if (id1 == 0 && id2 == 0 && id3 == 0)
            {
                return;
            }
            else
            {
                int stt = 0;
                //RAndom ghế hạng 1
                for (int i = 1; i <= id1; i++)
                {
                    ticket = new Ve();
                    stt++;
                    ticket.MaVe = "VE" + stt;
                    ticket.MaCB = cb.MaCB;
                    ticket.MaHV = "HV01";
                    ticket.TinhTrang = "Chưa đặt chỗ";
                    ticket.GiaVe = Convert.ToDouble(Request["dongiaG1"]);
                    ticket.CCCD = "null";
                    database.Ves.Add(ticket);
                    database.SaveChanges();
                }
                for (int i = 1; i <= id2; i++)
                {
                    ticket = new Ve();
                    stt++;
                    ticket.MaVe = "VE" + stt;
                    ticket.MaCB = cb.MaCB;
                    ticket.MaHV = "HV02";
                    ticket.TinhTrang = "Chưa đặt chỗ";
                    ticket.GiaVe = Convert.ToDouble(Request["dongiaG2"]);
                    ticket.CCCD = "null";
                    database.Ves.Add(ticket);
                    database.SaveChanges();
                }
                for (int i = 1; i <= id3; i++)
                {
                    ticket = new Ve();
                    stt++;
                    ticket.MaVe = "VE" + stt;
                    ticket.MaCB = cb.MaCB;
                    ticket.MaHV = "HV03";
                    ticket.TinhTrang = "Chưa đặt chỗ";
                    ticket.GiaVe = Convert.ToDouble(Request["dongiaG3"]);
                    ticket.CCCD = "null";
                    database.Ves.Add(ticket);
                    database.SaveChanges();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlan([Bind(Include = "MaMB,LoaiMayBay")] MayBay mb)
        {
            if (mb == null)
            {
                TempData["error"] = "Error";
                return RedirectToAction("Scheduleaflight");
            }
            else
            {
                database.MayBays.Add(mb);
                database.SaveChanges();
                return RedirectToAction("Scheduleaflight");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdddetailSchulea(ChiTietChuyenBay ct)
        {
            var checkmacb = (string)Session["mcb"];
            if (checkmacb != null)
            {
                ct.MaCTCB = checkmacb;
            }
            else
            {
                TempData["messageAlert"] = "erorr";
                return RedirectToAction("Scheduleaflight");
            }
            TempData["machuyenbay"] = ct.MaCTCB;
            TempData["messageAlert"] = "success";
            database.ChiTietChuyenBays.Add(ct);
            database.SaveChanges();
            return RedirectToAction("Scheduleaflight");
        }
        //Xem chi tiết chuyến bay
        public ActionResult Chitietcb(string id)
        {
            if (id != null)
            {
                var chitietcb = database.ChuyenBays.Find(id);
                return View(chitietcb);
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
        public ActionResult DetailFlight(string id)
        {
            if (id != null)
            {
                var thongtincb = database.ChuyenBays.Find(id);
                TempData["themthongtin"] = 2;
                return View(thongtincb);
            }
            else if (id == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailFlight([Bind(Include = "MaCB,MaMB,MaTbay,NgayGio,ThoiGianBay,SoLuongGheHang1,SoLuongGheHang2,SoLuongGheHang3,TinhTrang")] ChuyenBay cb,string id)
        {
            if (ModelState.IsValid)
            {
                Random rd = new Random();
                var ctcb = database.ChiTietChuyenBays.Where(s => s.MaCTCB == cb.MaCB).FirstOrDefault();
                ChiTietChuyenBay ctcb01 = new ChiTietChuyenBay();
                if (ctcb != null)
                {

                    ctcb.SanBayTrungGian = Request["SanBayTrungGian"];
                    ctcb.ThoiGianDung = Request["ThoiGianDung"];
                    ctcb.GhiChu = Request["GhiChu"];
                    database.Entry(ctcb).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();

                    database.Entry(cb).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();
                    TempData["machuyenbay"] = cb.MaCB;
                    TempData["messageAlert"] = "SuaThanhCong";
                }
                else
                {
                    if (id != null)
                    {
                        ctcb01.STT = rd.Next(0, 100000);
                        ctcb01.MaCTCB = cb.MaCB;
                        ctcb01.SanBayTrungGian = Request["SanBayTrungGian"];
                        ctcb01.ThoiGianDung = Request["ThoiGianDung"];
                        ctcb01.GhiChu = Request["GhiChu"];
                        database.ChiTietChuyenBays.Add(ctcb01);
                        database.SaveChanges();
                    }
                    else
                    {

                    }
                    database.Entry(cb).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();
                    TempData["machuyenbay"] = cb.MaCB;
                    TempData["messageAlert"] = "SuaThanhCong";
                }
                return RedirectToAction("Scheduleaflight");
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
                
        }
        [HttpPost]
        public ActionResult DeleteFlight(string id)
        {
            var tb = database.ChuyenBays.Find(id);
            database.ChuyenBays.Remove(tb);
            database.SaveChanges();
            TempData["machuyenbay"] = tb.MaCB;
            TempData["messageAlert"] = "XoaThanhCong";
            //return RedirectToAction("Scheduleaflight");
            return Json(new { success = true });
        }
        public ActionResult AddSchedulea()
        {
            return View();
        }

        public ActionResult FlightRoute()
        {
            return View();
        }
        [HttpPost]
        public JsonResult KTtenSB(string input)
        {
            bool isDuplicate = false;
            var check = database.SanBays.Where(s => s.MaSB == input).FirstOrDefault();
            if (check != null)
            {
                isDuplicate = true;
                return Json(isDuplicate, JsonRequestBehavior.AllowGet);
            }
            return Json(isDuplicate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetData()
        {
            bool proxyCreation = database.Configuration.ProxyCreationEnabled;
            try
            {
                database.Configuration.ProxyCreationEnabled = false;
                var dstb = database.TuyenBays.ToList();
                return Json(new { Data = dstb, TotalItems = dstb.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            finally
            {
                //restore ProxyCreation to its original state
                database.Configuration.ProxyCreationEnabled = proxyCreation;
            }
        }
        // Thêm AddAirport
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddAirport([Bind(Include = "MaSB,TenSB")] SanBay sb)
        //{
        //    if (sb == null)
        //    {
        //        TempData["error"] = "Error";
        //        return RedirectToAction("FlightRoute");
        //    }
        //    else
        //    {
        //        database.SanBays.Add(sb);
        //        database.SaveChanges();
        //        return RedirectToAction("FlightRoute");
        //    }

        //}

        //Áp dụng Ajax vào chức năng thêm mới sân bay
        [HttpPost]
        public ActionResult AddAirport(SanBay sb)
        {
            if (sb != null)
            {
                database.SanBays.Add(sb);

                try
                {
                    database.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["error"] = "Error";
                    return Json(new { success = false });
                }
            }
            TempData["error"] = "Error";
            return Json(new { success = false }); // Khai báo trả lại status của json
        }

        [HttpPost]
        public ActionResult AddRoute(TuyenBay tb)
        {
            if (tb == null)
            {
                TempData["messageAlert"] = "Error";
                return RedirectToAction("FlightRoute");
            }
            else
            {
                if (tb.SanBayDi == tb.SanBayDen)
                {
                    TempData["messageAlert"] = "Error01";
                    return RedirectToAction("FlightRoute");
                }
                //Check thông tin sân bay đi và sân bay đến có tồn tại sẵn trong hệ thống hay không 
                var check = database.TuyenBays.Where(s => s.SanBayDi == tb.SanBayDi && s.SanBayDen == tb.SanBayDen).FirstOrDefault();
                if(check != null)
                {
                    TempData["messageAlert"] = "Error02";
                    return RedirectToAction("FlightRoute");
                }
                else
                {
                    Random rd = new Random();
                    var matb = "TB" + rd.Next(1, 1000);
                    tb.MaTBay = matb;
                    database.TuyenBays.Add(tb);
                    database.SaveChanges();
                    TempData["matuyenbay"] = tb.MaTBay;
                    TempData["messageAlert"] = "success";
                    return RedirectToAction("FlightRoute");
                }
                //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DetailFR(string id)
        {
            var ttFR = database.TuyenBays.Find(id);
            return View(ttFR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailFR(TuyenBay tb)
        {
            database.Entry(tb).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            TempData["matuyenbay"] = tb.MaTBay;
            TempData["messageAlert"] = "SuaTBTC";
            return RedirectToAction("FlightRoute");
        }
        [HttpPost]
        public ActionResult DeleteFR(string id)
        {
            var tb = database.TuyenBays.Where(s => s.MaTBay == id).FirstOrDefault() ;
            database.TuyenBays.Remove(tb);
            database.SaveChanges();
            TempData["matuyenbay"] = tb.MaTBay;
            TempData["messageAlert"] = "XoaTBay";
            //return RedirectToAction("FlightRoute");
            return Json(new { success = true });
        }
        public ActionResult Plane()
        {
            return View();
        }
        //Thêm vé máy bay sau khi đã có chuyến bay
        public ActionResult TicketManagement()
        {
            var dsticket = database.Ves.ToList();
            return View(dsticket);
        }


        public ActionResult TicketList()
        {
            return View();
        }
        public ActionResult TotalRevenue()
        {
            var dshd = database.HoaDons.ToList().OrderByDescending(s=>s.NgayLap);
            return View(dshd);
        }
        public ActionResult DetailTotalRevenue(string id)
        {
            var cthd = database.ChiTietHDs.Where(s => s.MaHD == id).ToList();
            TempData["Mahd"] = id;
            return View(cthd);
        }
        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Myinfor()
        {
            NhanVien nhanvien = new NhanVien();

            // xuat ra thong tin nv vua dang nhap
            nhanvien = taikhoan();

            return View(nhanvien);
        }

        public ActionResult EditMyinfor(string id)
        {
            var nv = database.NhanViens.Find(id);

            //neu checkquyen == true thi la admin/ con = false thi la nvbv
            ViewBag.checkLogin = Checkquyen();

            return View(nv);
        }

        [HttpPost]
        public ActionResult EditMyinfor(NhanVien nhanvien)
        {
            if (Checkquyen() && ModelState.IsValid)
            {
                var nv = database.NhanViens.Find(nhanvien.IDNV);
                nv.MaCV = nhanvien.MaCV;
                nv.TenNV = nhanvien.TenNV;
                nv.NgaySinh = nhanvien.NgaySinh;
                nv.Password = nhanvien.Password;
                nv.UserName = nhanvien.UserName;
                nv.SDT_NV = nhanvien.SDT_NV;
                nv.GioiTinh = nhanvien.GioiTinh;
                database.SaveChanges();
            }

            return View("Myinfor", nhanvien);
        }
        [HttpPost]
        public ActionResult EditMyinforNV(string IDNV, string TenNV)
        {
            var nhanvien = database.NhanViens.Find(IDNV);
            if (!Checkquyen() && ModelState.IsValid)
            {
                NhanVien nv = database.NhanViens.Find(IDNV);

                nv.MaCV = nhanvien.MaCV;
                nv.TenNV = TenNV;
                nv.NgaySinh = nhanvien.NgaySinh;
                nv.Password = nhanvien.Password;
                nv.UserName = nhanvien.UserName;
                nv.SDT_NV = nhanvien.SDT_NV;
                nv.GioiTinh = nhanvien.GioiTinh;

                database.SaveChanges();
                
               
            }
            
            return View("Myinfor", nhanvien);
        }

        public ActionResult Staff()
        {
            return View();
        }
        public ActionResult DetailStaff(string id)
        {
            var ttnv = database.NhanViens.Find(id);
            return View(ttnv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailStaff(NhanVien nv)
        {
            database.Entry(nv).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            TempData["manhanvien"] = nv.IDNV;
            TempData["MessageAlert"] = "SuaNV";
            return RedirectToAction("Staff");

        }

        public ActionResult CustomerManagement()
        {
            return View();
        }


        public ActionResult DetailCus(string id)
        {
            var ttCus = database.KhachHangs.Find(id);
            return View(ttCus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailCus(KhachHang kh)
        {
            database.Entry(kh).State = (System.Data.Entity.EntityState)System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            TempData["makhachhang"] = kh.IDKH;
            TempData["MessageAlert"] = "SuaKH";
            return RedirectToAction("CustomerManagement");

        }

        public ActionResult Promotion()
        {
            return View();
        }
        
        public ActionResult CreatePromotion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePromotion(Voucher voucher, string SelectTT, string NgayApDung, string NgayHetHan)
        {

            voucher.TinhTrang = SelectTT;
            database.Vouchers.Add(voucher);
            database.SaveChanges();
            TempData["macv"] = voucher.MaVC;
            TempData["MessageAlert"] = "success";
            return RedirectToAction("Promotion");
        }



        [HttpPost]
        public JsonResult KTMavc(string input)
        {
            bool isDup = false;
            var check = database.Vouchers.Where(s => s.MaVC == input).FirstOrDefault();
            if (check != null)
            {
                isDup = true;
                return Json(isDup, JsonRequestBehavior.AllowGet);
            }
            return Json(isDup, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDatavc()
        {
            bool proxyCreation = database.Configuration.ProxyCreationEnabled;
            try
            {
                database.Configuration.ProxyCreationEnabled = false;
                var dstb = database.Vouchers.ToList();
                return Json(new { Data = dstb, TotalItems = dstb.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            finally
            {
                //restore ProxyCreation to its original state
                database.Configuration.ProxyCreationEnabled = proxyCreation;
            }
        }


        public ActionResult DeletePro(string id)
        {
            var tb = database.Vouchers.Find(id);
            database.Vouchers.Remove(tb);
            database.SaveChanges();
            TempData["mavc"] = tb.MaVC;
            TempData["messageAlert"] = "XoaThanhCong";
            //return RedirectToAction("FlightRoute");
            return Json(new { success = true });
        }


        
        //Bảng log ghi lại lịch sử thêm xóa chuyến bay
        public ActionResult History()
        {
            return View();
        }

        public ActionResult CreateStaff()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateStaff(NhanVien nhanVien, string IDNV, string MaCV, string UserName, string Password, string TenNV, string SDT_NV, string GioiTinh)
        {

            database.NhanViens.Add(nhanVien);
            database.SaveChanges();
            TempData["manhanvien"] = nhanVien.IDNV;
            TempData["MessageAlert"] = "themthanhcong";
            return RedirectToAction("Staff");
        }
        public ActionResult DeleteStaff(string id)
        {
            var st = database.NhanViens.Find(id);
            database.NhanViens.Remove(st);
            database.SaveChanges();
            TempData["manhanvien"] = st.IDNV;
            TempData["messageAlert"] = "XoaThanhCong";

            return Json(new { success = true });
        }

    }
}
