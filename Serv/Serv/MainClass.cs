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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv
{
    class MainClass
    {
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

            Serv serv = new Serv();
            serv.Start("127.0.0.1", 1234);

            while(true)
            {
                string str = Console.ReadLine();
                switch(str)
                {
                    case "quit":
                        return;
                }
            }
        }
    }
}
