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
    
    public partial class THEATRECOMPLEX
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THEATRECOMPLEX()
        {
            this.MOVIEHALLs = new HashSet<MOVIEHALL>();
        }
    
        public int TheatreID { get; set; }
        public string TheatreName { get; set; }
        public string TheatreContact { get; set; }
        public string TheatreAddress { get; set; }
        public string TheatreLocation { get; set; }
        public int MovieHallCount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MOVIEHALL> MOVIEHALLs { get; set; }
    }
}
