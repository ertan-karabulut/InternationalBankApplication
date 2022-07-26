using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class CardPassword
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public virtual Card Card { get; set; }
    }
}
