using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository {

        //This sets the context of the child class to the context of the super class
        public ChatRepository(ZewwDbContext context) : base(context) { }


        public void Insert(User userToAdd) {
            throw new NotImplementedException();
        }

        //Your methods go here
        
        public void addChat(Chat chatToAdd) {
            dbSet.Add(chatToAdd);
        }

    }
}
