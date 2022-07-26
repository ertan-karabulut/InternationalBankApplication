using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class InternetPassword
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Password { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
