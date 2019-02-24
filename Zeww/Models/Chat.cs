using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zeww.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
