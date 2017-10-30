using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CSharp直接连接MySQL
{
    class Program
    {
        
        static void Main(string[] args)
        {
            // Insert();
            //Update();
            //Delete();
            //  ReadUsersCount();

            //Read();

            Console.WriteLine( verifyUser("4", "123"));
           
        }

        static void Read()
        {
            String myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                string sql = "select * from users";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                //cmd.ExecuteNonQuery();
                //cmd.ExecuteScalar();

                //reader.Read();

                //reader.Read();
                //Console.WriteLine( reader[0].ToString() + reader[1].ToString());
              

                while (reader.Read())
                {


                    //Console.WriteLine(reader.GetInt32(1).ToString()+ reader.GetInt32(2).ToString());
                    Console.WriteLine(reader.GetString("id").ToString()+ "  "+  reader.GetString("username").ToString());


                    //  Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                }
                Console.WriteLine("已经建立连接");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }
        static void Insert()
        {
            String myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                string sql = "Insert into users(username,password,registerdate) values('wu1','wu','"+ DateTime.Now + "')";
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int rest =  cmd.ExecuteNonQuery();//返回值是数据库中受影响的记录

                Console.WriteLine("已经建立连接" + rest.ToString());
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            { 
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message); 
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }

        static void Update()
        {
            String myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                string sql = "update users set username='xx' where username='wu'";
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int rest = cmd.ExecuteNonQuery();//返回值是数据库中受影响的记录



                Console.WriteLine("已经建立连接" + rest.ToString());
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }
        static void Delete()
        {
            String myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                string sql = "delete  from users where username='wu1'";
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int rest = cmd.ExecuteNonQuery();//返回值是数据库中受影响的记录
                



                Console.WriteLine("已经建立连接" + rest.ToString());
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }
        static void ReadUsersCount()
        {
            string myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                string sql = "select count(*) from users";
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
               // MySqlDataReader read = cmd.ExecuteReader();
                Object o =  cmd.ExecuteScalar();
                int count = Convert.ToInt32(o.ToString());
                Console.WriteLine(count);

                //  read.Read();
                //  //while(read.Read())
                //  //{
                //     int count =  Convert.ToInt32(read[0].ToString());
                //      Console.WriteLine(count);
                ////  }
                Console.WriteLine("已经建立连接" );
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }
        static bool verifyUser(string username, string password)
        {
            String myConnectionstring;
            myConnectionstring = "server=127.0.0.1;uid=root;" + "pwd=wujunqiang;database=mygamedb";
            MySqlConnection conn = new MySqlConnection(myConnectionstring);

            try
            {
                conn.Open();
                //string sql = "select * from users where username='" + username +"' and password='" + password + "';";
                string sql = "select * from users where username=@username and password=@password";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                MySqlDataReader reader = cmd.ExecuteReader();
                //cmd.ExecuteNonQuery();
                //cmd.ExecuteScalar();

                //reader.Read();

                //reader.Read();
                //Console.WriteLine( reader[0].ToString() + reader[1].ToString());\

                if (reader.Read())
                {
                    return true;
                }
                //Console.WriteLine("已经建立连接");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                // MessageBox.Show(ex.Message);
                Console.WriteLine("连接失败" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
    }
   
}
