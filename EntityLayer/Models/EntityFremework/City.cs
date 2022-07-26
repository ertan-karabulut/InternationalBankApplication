using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class City
    {
        public City()
        {
            Adresses = new HashSet<Adress>();
            Branches = new HashSet<Branch>();
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        public int? CityNumber { get; set; }
        public string CityName { get; set; }
        public int? CountryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
