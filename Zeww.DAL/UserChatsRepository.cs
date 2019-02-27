using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    class UserChatsRepository : GenericRepository<UserChats>, IUserChatsRepository
    {
        public UserChatsRepository(ZewwDbContext context) : base(context) { }
    }
}
