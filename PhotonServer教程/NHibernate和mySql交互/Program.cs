using NHibernate.Cfg;
using System;
using NHibernate;
using NHibernateAndMySql.Model;
using NHibernateAndMySql.Manager;
using System.Collections.Generic;

namespace NHibernateAndMySql
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configuration configuration = new Configuration();
            //configuration.Configure();//解析XML文件，主配置
            //configuration.AddAssembly("NHibernateAndMySql");//解析 映射文件 user.hbm.xml

            //ISessionFactory sessionFactory = null;
            //ISession session = null;
            //ITransaction transaction = null;


            //try
            //{
            //    sessionFactory = configuration.BuildSessionFactory();

            //    session = sessionFactory.OpenSession();//打开一个跟数据库的会话

            //    //User user = new User() { Username = "root", Password = "wujunqiang" };

            //    //session.Save(user);

            //    //事务
            //    transaction =  session.BeginTransaction();

            //    User user1 = new User() { Username = "111", Password = "wujunqiang" };
            //    User user2 = new User() { Username = "333", Password = "wujunqiang" };

            //    session.Save(user1);
            //    session.Save(user2);

            //    transaction.Commit();


            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //finally
            //{
            //    if(transaction!=null)
            //    {
            //        transaction.Dispose();
            //    }
            //    if(session != null)
            //    {
            //        session.Close();
            //    }
            //    if (sessionFactory != null)
            //    {

            //        sessionFactory.Close();
            //    }
            //}

            //User user = new User() { Username = "ahsdjfhakj",Password="ahdsuf" };
            //IUserManager userManager = new UserManger();
            //userManager.Add(user);
           // User user = new User() { Username="44"};
            IUserManager userManager = new UserManger();
            //userManager.Update(user);
            //userManager.Remove(user);
            // User user = userManager.GetByUsername("44");
            //ICollection<User> users = userManager.GetAllUsers();
            //foreach(User u in users)
            //{
            //    Console.WriteLine(u.Username + "  " + u.Password);
            //}
            Console.WriteLine(userManager.VerifyUser("44", "123"));
            //Console.WriteLine(user.Username);
            Console.ReadKey();
        }
    }
}
