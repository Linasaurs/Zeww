using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class FileRepository : GenericRepository<File>, IFileRepository
    {

        //This sets the context of the child class to the context of the super class
        public FileRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here
        public IEnumerable<File> GetFilesBySenderName(string senderName , string chatName)
        {
            if (!String.IsNullOrEmpty(chatName))
            {
                if (!String.IsNullOrEmpty(senderName))
                    return Get(FilterBySenderName(senderName, chatName));
                return GetFilesFromChat(chatName);
            }
            else
                return null;
           
        }

        public void Add(File fileToAdd)
        {
            dbSet.Add(fileToAdd);
        }

        private Expression<Func<File, bool>> FilterBySenderName(string name, string channelName)
        {
            return File => (File.User.Name == name && File.Chat.Name == channelName);
        }

        public IEnumerable<File> GetFilesFromChat(string chatName)
        {
            return Get(File => (File.Chat.Name == chatName));
        }
    }
}
