
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
    
public partial class HangVe
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public HangVe()
    {

        this.Ves = new HashSet<Ve>();

    }


    public string MaHV { get; set; }

    public string TenHV { get; set; }

    public Nullable<double> PhuPhi { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Ve> Ves { get; set; }

}

}
