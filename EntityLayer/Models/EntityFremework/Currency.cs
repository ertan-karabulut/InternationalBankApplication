using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Currency
    {
        public Currency()
        {
            Accounts = new HashSet<Account>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IconPath { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
