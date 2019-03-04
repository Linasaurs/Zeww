﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int SenderID { get; set; }
        public int ChatId { get; set; }
        public string MessageContent { get; set; }
        public bool isPinned { get; set; }
        public DateTime dateTime { set; get; }

    }
}
