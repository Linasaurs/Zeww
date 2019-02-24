using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class UserWorkspace
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }
    }
}
