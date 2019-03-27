using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IUserChatsRepository : IGenericRepository<UserChats>
    {
        //Your method headers go here
        UserChats GetUserChatByIds(int userID, int chatId);
        int GetNumberOfUsersInChat(int? chatId);

        bool IsUserInChannel(int userId, int chatId);
        IQueryable<int> GetCommonChats(int userId1, int userId2);
    }
}

