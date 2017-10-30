﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernateAndMySql.Model;

namespace NHibernateAndMySql.Manager
{
    interface IUserManager
    {
        void Add(User user);
        void Update(User user);
        void Remove(User user);
        User GetById(int id);
        User GetByUsername(string username);
        ICollection<User> GetAllUsers();
        bool VerifyUser(string username, string password);
    }
}
