using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class Workspace
    {
        public Workspace() {
            this.UserWorkspaces = new HashSet<UserWorkspace>();
        }

        [Key]
        public int Id { get; set; }

        [MinLength(3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required]
        public string WorkspaceName { get; set; } 

        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; }

    }
}
