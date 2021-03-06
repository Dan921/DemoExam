//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoExamApp.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class RealEstate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RealEstate()
        {
            this.Offers = new HashSet<Offer>();
        }
    
        public int Id { get; set; }
        public string CityAddress { get; set; }
        public string StreetAddress { get; set; }
        public string HouseAddress { get; set; }
        public string ApartmentNumberAddress { get; set; }
        public Nullable<double> CoordinateLatitude { get; set; }
        public Nullable<double> CoordinateLongitude { get; set; }
        public Nullable<int> Floor { get; set; }
        public Nullable<int> Rooms { get; set; }
        public Nullable<double> TotalArea { get; set; }
        public Nullable<int> TotalFloors { get; set; }
        public string Type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
