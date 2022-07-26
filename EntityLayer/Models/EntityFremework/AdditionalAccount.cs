using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class AdditionalAccount
    {
        public AdditionalAccount()
        {
            AdditionalAccountHistories = new HashSet<AdditionalAccountHistory>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Limit { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<AdditionalAccountHistory> AdditionalAccountHistories { get; set; }
    }
}
