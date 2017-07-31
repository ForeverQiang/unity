/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 7/28/2017 5:58:10 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Serv
{
    class MainClass
    {
        static string str = "";
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");

            ////Socket
            //Socket listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////bind
            //IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            //IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);
            //listenfd.Bind(ipEp);
            ////Listen
            //listenfd.Listen(0);
            //Console.WriteLine("【服务器】启动成功");
            //while(true)
            //{
            //    //Accept
            //    Socket connfd = listenfd.Accept();
            //    Console.WriteLine("[服务器]Accept");
            //    //Recv
            //    byte[] readBuff = new byte[1024];
            //    int count = connfd.Receive(readBuff);
            //    string str = System.Text.Encoding.UTF8.GetString(readBuff, 0, count);
            //    //send
            //    byte[] bytes = System.Text.Encoding.Default.GetBytes("serv echo" + str);
            //    connfd.Send(bytes);
            //}

            //Serv serv = new Serv();
            //serv.Start("127.0.0.1", 1234);

            //while(true)
            //{
            //    string str = Console.ReadLine();
            //    switch(str)
            //    {
            //        case "quit":
            //            return;
            //    }
            //}

            //Player player = new Player()
            //{
            //    coin = 1,
            //    money = 1,
            //    name = "xiaoming"
            //};
            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream("data.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            //formatter.Serialize(stream, player);
            //stream.Close();


            //Timer timer = new Timer();
            //timer.AutoReset = true;
            //timer.Interval = 1000;
            //timer.Elapsed += new ElapsedEventHandler(Tick);
            //timer.Start();
            ////不要推出程序
            //Console.Read();

            Thread t1 = new Thread(Add1);
            t1.Start();
            Thread t2 = new Thread(Add2);
            t2.Start();
            //等待一段时间
            Thread.Sleep(1000);
            //输出
            Console.WriteLine(str);
        }

        //线程1
        public static void Add1()
        {
            lock(str)
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(10);
                    str += "A";
                }
            }
            
        }
        //线程2
        public static void Add2()
        {
            lock(str)
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(10);
                    str += "B";
                }
            }
            
        }

        public static void Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("每秒执行一次");
        }
    }
}
