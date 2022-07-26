using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class CreditCard
    {
        public CreditCard()
        {
            CardBalanceHistories = new HashSet<CardBalanceHistory>();
            CardBalances = new HashSet<CardBalance>();
        }

        public int CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public decimal CreditCardLimit { get; set; }

        public virtual Card CreditCardNavigation { get; set; }
        public virtual ICollection<CardBalanceHistory> CardBalanceHistories { get; set; }
        public virtual ICollection<CardBalance> CardBalances { get; set; }
    }
}
