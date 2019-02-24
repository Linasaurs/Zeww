using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL {
    public class MessageRepository: GenericRepository<Message>, IMessageRepository {
        //This sets the context of the child class to the context of the super class
        public MessageRepository(ZewwDbContext context) : base(context) { }
        
        //Your methods go here

    }


}