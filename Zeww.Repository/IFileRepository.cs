using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IFileRepository : IGenericRepository<File>
    {
        //Your method headers go here
        IEnumerable<File> GetFilesBySenderName(string name);
    }
}
