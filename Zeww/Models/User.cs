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
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "name must be between 3 and 15 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "user name must be between 3 and 15 characters")]
        public string UserName{ get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "password must be between 3 and 15 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; }
        public virtual ICollection<UserChats> UserChats { get; set; }

        public User() {
            this.UserWorkspaces = new HashSet<UserWorkspace>();
            this.UserChats = new HashSet<UserChats>();
        }
       
    }
    public enum Status {
        Available,
        Busy,
        Away
    }
}
