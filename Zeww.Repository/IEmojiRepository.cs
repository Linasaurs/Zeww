using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IEmojiRepository : IGenericRepository<Emoji>
    {
        void Add(Emoji emoji);
        void DeleteEmoji(int messageId, int userId);
        int GetEmojiCount(int messageId);
    }
}
