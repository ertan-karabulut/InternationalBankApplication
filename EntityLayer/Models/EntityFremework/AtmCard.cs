using System;
using System.Collections.Generic;

#nullable disable

namespace EntityLayer.Models.EntityFremework
{
    public partial class AtmCard
    {
        public int AtmCardId { get; set; }
        public int AccountId { get; set; }

        public virtual Card AtmCardNavigation { get; set; }
    }
}
