﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Zeww.Repository {
    public interface IUnitOfWork {
        IChatRepository Chats { get; }
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }
        IUserWorkspaceRepository UserWorkspaces { get; }
        IWorkspaceRepository Workspaces { get; }

        void Save();
    }
}
