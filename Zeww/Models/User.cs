using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; }

        public User() {
            this.UserWorkspaces = new HashSet<UserWorkspace>();
        }
    }
}
