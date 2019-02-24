using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;
using Microsoft.AspNetCore.Identity;

namespace Zeww.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository {
        //This sets the context of the child class to the context of the super class
        public UserRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here
        public void Add(User userToAdd) {
            dbSet.Add(userToAdd);
        }

        public User Authenticate(string email, string password)
        {
            var users = Get(u => u.Email == email);
            var user = users.GetEnumerator().Current;
            if(user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;

        }
    }
}