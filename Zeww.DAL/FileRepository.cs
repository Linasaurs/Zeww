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
        public IEnumerable<File> GetFilesBySenderName(string senderName , string channelName)
        {
            if (!String.IsNullOrEmpty(channelName))
            {
                if (!String.IsNullOrEmpty(senderName))
                    return Get();//FilterBySenderName(senderName,channelName)
                return Get();
            }
            else
                return null;
           
        }

        /*private Expression<Func<File, bool>> FilterBySenderName(string name, string channelName)
        {
            return File => (File.User.Name == name && File.Chat.Name == channelName);
        }*/

       
    }
}
