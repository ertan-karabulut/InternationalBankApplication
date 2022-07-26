﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto.Card
{
    public class AtmCardDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
