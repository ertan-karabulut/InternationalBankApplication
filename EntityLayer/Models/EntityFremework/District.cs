using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class District
    {
        public District()
        {
            Adresses = new HashSet<Adress>();
            Branches = new HashSet<Branch>();
        }

        public int Id { get; set; }
        public string DistrictName { get; set; }
        public int? CityId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
    }
}
