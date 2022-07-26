﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Dto.Mail
{
    public class MailUpdateDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string EMail { get; set; }
        public bool? IsFavorite { get; set; }
        public bool? IsActive { get; set; }
    }
}
