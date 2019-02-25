using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Workspace")]
        public int WorkspaceId { get; set; }
        [ForeignKey("User")]
        public int CreatorID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        //Purpose should only be added in the case pf group channels
        public string Purpose { get; set; }
        public virtual ICollection<UserChats> UserChats { get; set; }

        public Chat()
        {
            this.UserChats = new HashSet<UserChats>();
            this.DateCreated = DateTime.Now;
        }
    }
}
