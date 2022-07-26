using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Adress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string AdressName { get; set; }
        public string AdressDetail { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int CountryId { get; set; }
        public DateTime? DomicileStartDate { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual District District { get; set; }
    }
}
