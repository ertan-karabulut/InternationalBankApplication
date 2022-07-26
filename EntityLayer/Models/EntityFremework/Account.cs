using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Account
    {
        public Account()
        {
            AccountBalanceHistories = new HashSet<AccountBalanceHistory>();
            AccountBalances = new HashSet<AccountBalance>();
            AdditionalAccounts = new HashSet<AdditionalAccount>();
        }

        public int Id { get; set; }
        public int BranchId { get; set; }
        public int CustomerId { get; set; }
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
        public int TypeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CurrencyUnitId { get; set; }
        public bool IsActive { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Currency CurrencyUnit { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual AccountType Type { get; set; }
        public virtual ICollection<AccountBalanceHistory> AccountBalanceHistories { get; set; }
        public virtual ICollection<AccountBalance> AccountBalances { get; set; }
        public virtual ICollection<AdditionalAccount> AdditionalAccounts { get; set; }
    }
}
