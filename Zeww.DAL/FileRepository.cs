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

        public IEnumerable<File> GetFilesBySenderName(string name)
        {
            if(!String.IsNullOrEmpty(name))
            {
                return Get(FilterBySenderName(name));
            }

            return Get();
        }

        private Expression<Func<File, bool>> FilterBySenderName(string name)
        {
            return File => File.User.Name == name;
        }

        //Your methods go here
    }
}
