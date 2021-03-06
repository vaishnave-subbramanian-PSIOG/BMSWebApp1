//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMSDbEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BOOKING
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BOOKING()
        {
            this.PAYMENTDETAILS = new HashSet<PAYMENTDETAIL>();
            this.MOVIEHALLSEATs = new HashSet<MOVIEHALLSEAT>();
        }
    
        public int BookingID { get; set; }
        public System.DateTime BookingDate { get; set; }
        public int CustomerID { get; set; }
        public int ShowID { get; set; }
        public System.DateTime ShowDate { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual SHOWINFO SHOWINFO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAYMENTDETAIL> PAYMENTDETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOVIEHALLSEAT> MOVIEHALLSEATs { get; set; }
    }
}
