using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Branch
    {
        public Branch()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string BranchNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchAdress { get; set; }
        public int? DistrictId { get; set; }
        public int CityId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
