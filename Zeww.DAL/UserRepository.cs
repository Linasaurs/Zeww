using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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

        public bool Authenticate(User user, string claimedPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, claimedPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }
            return true;
        }

        public string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}