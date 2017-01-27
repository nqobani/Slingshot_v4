﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slingshot.Data.Models
{
    public class Attachment
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("emailId")]
        public long emailId { get; set; }
        public string name { get; set; }
        public string file { get; set; }
    }
}
