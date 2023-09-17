using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingAirline.Models
{
    public class Customer
    {
        public string idVe { get; set; }

        public string HoTenKH { get; set; }
        public DateTime NgaySinh { get; set; }
        public string EmailKH { get; set; }
        public string SDT { get; set; }
        public string GioiTinh { get; set; }
        public string CCCD { get; set; }
    }
    public class DanhSachKH
    {
        List<Customer> items = new List<Customer>();
        public IEnumerable<Customer> Items
        {
            get { return items; }
        }
        //Thêm thông tin KH  vào danh sách
        public void Add(string idve, string hotenkh, DateTime ngaysinh, string emailkh, string dt, string gioitinh, string cccd)
        {
            var item = Items.FirstOrDefault(s => s.idVe == idve);
            items.Add(new Customer
            {
                idVe = idve,
                HoTenKH = hotenkh,
                NgaySinh = ngaysinh,
                EmailKH = emailkh,
                SDT = dt,
                GioiTinh = gioitinh,
                CCCD = cccd
            });
        }

    }
}