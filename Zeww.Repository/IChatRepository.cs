﻿using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IChatRepository : IGenericRepository<Chat> {

        //Your method headers go here
        void GetListOfChannelsbyUserId(int id);

    }
}
