
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
    
public partial class TuyenBay
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public TuyenBay()
    {

        this.ChuyenBays = new HashSet<ChuyenBay>();

    }


    public string MaTBay { get; set; }

    public string SanBayDi { get; set; }

    public string SanBayDen { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ChuyenBay> ChuyenBays { get; set; }

    public virtual SanBay SanBay { get; set; }

    public virtual SanBay SanBay1 { get; set; }

}

}
