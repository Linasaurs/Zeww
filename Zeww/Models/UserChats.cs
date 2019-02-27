using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class UserChats
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
    }
}
