using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository {
        //This sets the context of the child class to the context of the super class
        public UserRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here
        public User GetUserByUserName(string name)
        {
            IQueryable<User> query = dbSet;
            return query.SingleOrDefault(u => u.Name == name);
        }

        public User GetUserByEmail(string email)
        {
            IQueryable<User> query = dbSet;
            return query.SingleOrDefault(u => u.Email == email);
        }

    }
}