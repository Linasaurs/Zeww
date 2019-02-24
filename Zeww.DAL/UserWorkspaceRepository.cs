using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class UserWorkspaceRepository : GenericRepository<UserWorkspace>, IUserWorkspaceRepository {
        //This sets the context of the child class to the context of the super class
        public UserWorkspaceRepository(ZewwDbContext context) : base(context) { }

        //This is a junction table, no methods go here!
    }
}
