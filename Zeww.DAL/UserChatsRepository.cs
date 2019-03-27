using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    class UserChatsRepository : GenericRepository<UserChats>, IUserChatsRepository
    {
        public UserChatsRepository(ZewwDbContext context) : base(context) { }

        public UserChats GetUserChatByIds(int userId, int chatId)
        {
            IQueryable<UserChats> query = dbSet;

            return query.Where(uc => uc.UserId == userId && uc.ChatId == chatId).SingleOrDefault();
        }

        public bool IsUserInChannel(int userId, int chatId)
        {
            IQueryable<UserChats> query = dbSet;

            return query.Any(uc => uc.UserId == userId && uc.ChatId == chatId);
        }

        public IQueryable<int> GetCommonChats(int userId1, int userId2)
        {
            IQueryable<UserChats> query = dbSet;

            var user1userChats = query.Where(uc => uc.UserId == userId1);
            var user2userChats = query.Where(uc => uc.UserId == userId2);

            var commonChats = from uc1 in user1userChats
                    join uc2 in user2userChats
                    on uc1.ChatId equals uc2.ChatId
                    select uc1.ChatId;

            return commonChats;
        }
    }
}
