
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace BookingAirline.Models
{

using System;
    using System.Collections.Generic;
    
public partial class PhieuDatCho
{

    public string MaPhieu { get; set; }

    public string MaCB { get; set; }

    public string IDKH { get; set; }

    public string CCCD { get; set; }

    public Nullable<System.DateTime> NgayDat { get; set; }

    public string SoGhe { get; set; }



    public virtual ChuyenBay ChuyenBay { get; set; }

}

}
