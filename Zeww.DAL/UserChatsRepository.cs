using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class UserChatsRepository : GenericRepository<UserChats>, IUserChatsRepository
    {
            public UserChatsRepository(ZewwDbContext context) : base(context) { }

            //Your methods go here

     }
}
