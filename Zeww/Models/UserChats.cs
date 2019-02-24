using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class UserChats
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        public int ChatId { get; set; }
        public Chat UserChat { get; set; }
    }
}
