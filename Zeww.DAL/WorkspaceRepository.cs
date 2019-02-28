using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class WorkspaceRepository : GenericRepository<Workspace>, IWorkspaceRepository
    {
        //This sets the context of the child class to the context of the super class
        public WorkspaceRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here 
        public List<int> GetUsersIdInWorkspace(int id)
        {
            IQueryable<Workspace> queryWorksapces = dbSet;
            var workspaces= queryWorksapces.Include(w=>w.UserWorkspaces).ToList();
            var listOfUsersIds = new List<int>();
            foreach (var userWorkspace in workspaces[0].UserWorkspaces)
            {
                listOfUsersIds.Add(userWorkspace.UserId);
            }
            return listOfUsersIds;
        }

    }
}
