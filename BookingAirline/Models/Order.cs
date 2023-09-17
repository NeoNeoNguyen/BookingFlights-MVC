using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingAirline.Models
{
    public class Order
    {
        public DateTime CreateDate { get; set; }
        public string NumberPhone { get; set; }
        public string ShipName { get; set; }
        public string ShipEmail { get; set; }
        public string CCCD { get; set; }
        public double Total { get; set; }
    }
}