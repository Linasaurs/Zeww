using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class ChatRepository : GenericRepository<Chat> , IChatRepository{

        //This sets the context of the child class to the context of the super class
        public ChatRepository(ZewwDbContext context) : base(context) { }
        
        //Your methods go here
        public void GetListOfChannelsbyUserId(int id)
        {
            //Logic
            GetByID(id);
        
        }
    }
}
