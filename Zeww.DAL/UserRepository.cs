using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository {
        //This sets the context of the child class to the context of the super class
        public UserRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here
        public void Add(User userToAdd) {
            dbSet.Add(userToAdd);
        }
    }
}