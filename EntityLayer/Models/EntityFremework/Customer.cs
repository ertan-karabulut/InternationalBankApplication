using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Adresses = new HashSet<Adress>();
            Cards = new HashSet<Card>();
            CustomerRoles = new HashSet<CustomerRole>();
            EMails = new HashSet<EMail>();
            InternetPasswords = new HashSet<InternetPassword>();
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        public int Id { get; set; }
        public string CustomerNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? CreateDate { get; set; }
        public short Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Photo { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<CustomerRole> CustomerRoles { get; set; }
        public virtual ICollection<EMail> EMails { get; set; }
        public virtual ICollection<InternetPassword> InternetPasswords { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
