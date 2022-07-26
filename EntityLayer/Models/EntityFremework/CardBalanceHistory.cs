﻿using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class CardBalanceHistory
    {
        public int Id { get; set; }
        public int CreditCardId { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }
        public decimal Debit { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual CreditCard CreditCard { get; set; }
    }
}
