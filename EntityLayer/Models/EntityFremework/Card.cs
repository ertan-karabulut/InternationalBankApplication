using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class Card
    {
        public Card()
        {
            CardPasswords = new HashSet<CardPassword>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual AtmCard AtmCard { get; set; }
        public virtual CreditCard CreditCard { get; set; }
        public virtual ICollection<CardPassword> CardPasswords { get; set; }
    }
}
