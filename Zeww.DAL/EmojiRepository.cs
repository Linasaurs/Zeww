using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class EmojiRepository : GenericRepository<Emoji>, IEmojiRepository
    {
        public EmojiRepository(ZewwDbContext context) : base(context)
        {
        }

        public void Add(Emoji emoji)
        {
            dbSet.Add(emoji);
        }

        public void DeleteEmoji(int messageId, int userId)
        {
            Emoji emoji = dbSet.SingleOrDefault(e => e.messageID == messageId && e.userID == userId);
            dbSet.Remove(emoji);
        }

        public int GetEmojiCount(int messageId)
        {
            return dbSet.Where(e => e.messageID == messageId).Count();
        }
    }
}
