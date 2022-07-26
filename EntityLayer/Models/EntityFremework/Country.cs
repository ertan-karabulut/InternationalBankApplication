using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Country
    {
        public Country()
        {
            Adresses = new HashSet<Adress>();
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
