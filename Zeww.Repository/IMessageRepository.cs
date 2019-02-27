using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository {
    public interface IMessageRepository : IGenericRepository<Message> {

        void Add(Message message);
        void PinMessage(int messageId);
    }
}
