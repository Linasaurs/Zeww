using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IUserRepository : IGenericRepository<User> {

        //Your method headers go here
        void Add(User userToAdd);

        
    }
}
