﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.DAL
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        void Save();
        void Dispose(bool disposing);
        void Dispose();
    }
}
