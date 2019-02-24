using System;
using System.Collections.Generic;
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
    }
}
