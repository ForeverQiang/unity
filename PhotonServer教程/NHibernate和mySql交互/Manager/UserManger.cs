/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/19/2017 9:47:25 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernateAndMySql.Model;
using NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace NHibernateAndMySql.Manager
{
    class UserManger : IUserManager
    {
        public void Add(User user)
        {
            //ISession session = NHibrenateHelper.OpenSession();
            //session.Save(user);
            //session.Close();

            using (ISession session = NHibrenateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                //ICriteria criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Eq("Name", username));
                //User user =  criteria.UniqueResult<User>();

                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();
                return users;

            }
        }

        public Model.User GetById(int id)
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    User user =  session.Get<User>(id);
                    transaction.Commit();
                    return user;
                }
            }
        }

        public User GetByUsername(string username)
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                //ICriteria criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Eq("Name", username));
                //User user =  criteria.UniqueResult<User>();

                User user =  session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", username)).UniqueResult<User>();
                return user;

            }
        }

        public void Remove(User user)
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }

        public bool VerifyUser(string username, string password)
        {
            using (ISession session = NHibrenateHelper.OpenSession())
            {
                User user = session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();
                if (user == null)
                    return false;
                return true;
            }
        }
    }
}
