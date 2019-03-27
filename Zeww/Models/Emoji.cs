using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zeww.Models
{
    public class Emoji
    {
        [Key]
        public int emojiID { set; get; }
        [ForeignKey("Message")]
        public int messageID { set; get; }
        [ForeignKey("User")]
        public int userID { set; get; }
    }
}
