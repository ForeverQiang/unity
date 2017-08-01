/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/1/2017 10:17:18 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明： 服务器处理网络连接的部分   Socker->Bind->Accept->接收和处理->关闭连接
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv.core
{
    public class ServNet
    {
        //监听套接字
        public Socket listenfd;
        //客户端连接
        public Conn[] conns;
        //最大连接数
        public int maxConn = 50;
        //单例
        public static ServNet instance;

        public ServNet()
        {
            instance = this;
        }

        //获取连接池索引，返回负数表示获取失败
        public int NewIndex()
        {
            if (conns == null)
                return -1;
            for(int i = 0;i < conns.Length; i ++)
            {
                if(conns[i] == null)
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if(conns[i].isUse == false)
                {
                    return i;
                }
            }
            return -1;
        }

        //开启服务器
        public void Start(string host, int port)
        {
            //连接池
            conns = new Conn[maxConn];
            for(int i = 0; i  < maxConn; i++ )
            {
                conns[i] = new Conn();
            }

            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Bind
            IPAddress ipAdr = IPAddress.Parse(host);
            IPEndPoint ipEp = new IPEndPoint(ipAdr, port);
            listenfd.Bind(ipEp);
            //listen
            listenfd.Listen(maxConn);
            //Accept
            listenfd.BeginAccept(AcceptCb, null);
            Console.WriteLine("[服务器]启动成功");
        }

        //Accept回调
        public void AcceptCb(IAsyncResult ar)
        {
            try
            {
                Socket socket = listenfd.EndAccept(ar);
                int index = NewIndex();

                if(index < 0)
                {
                    socket.Close();
                    Console.WriteLine("[警告]连接已满");
                }
                else
                {
                    Conn conn = conns[index];
                    conn.Init(socket);
                    string adr = conn.GetAdress();
                    Console.WriteLine("客户端连接 [" + adr + "] conn池ID：" + index);
                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);
                }
                listenfd.BeginAccept(AcceptCb, null);
            }
            catch(Exception e)
            {
                Console.WriteLine("Accept失败:" + e.Message);
            }
        }

        //ReceiveCB回调
        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            lock(conn)
            {
                try
                {
                    int count = conn.socket.EndReceive(ar);
                    //关闭信号
                    if (count <= 0)
                    {
                        Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开连接");
                        conn.Close();
                        return;
                    }
                    conn.buffCount += count;
                    ProcessData(conn);
                    //继续接收
                  //  conn.socket.BeginReceive(参数);
                }
                catch(Exception e)
                {
                    Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开连接");
                    conn.Close();
                }
            }
        }

        public void ProcessData(Conn conn)
        {
            //小于长度字节
            if(conn .buffCount < sizeof(Int32))
            {
                return;
            }
            //消息长度
            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);
            if(conn.buffCount < conn.msgLength + sizeof(Int32))
            {
                return;
            }
            //处理消息
            string str = System.Text.Encoding.UTF8.GetString(conn.readBuff, sizeof(Int32), conn.msgLength);
            Console.WriteLine("收到消息[ " + conn.GetAdress() + "] " + str);
            Send(conn, str);
            //清除已处理的消息
            int count = conn.buffCount - conn.msgLength - sizeof(Int32);
            Array.Copy(conn.readBuff, sizeof(Int32) + conn.msgLength, conn.readBuff, 0, count);
            conn.buffCount = count;
            if(conn.buffCount > 0)
            {
                ProcessData(conn);
            }
        }

        //发送
        public void Send(Conn conn,string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] sendbuff = length.Concat(bytes).ToArray();
            try
            {
                conn.socket.BeginSend(sendbuff, 0, sendbuff.Length, SocketFlags.None, null, null);
            }
            catch(Exception e)
            {
                Console.WriteLine("[发送消息]" + conn.GetAdress() + " : " + e.Message);
            }
        }

        //关闭
        public void Close()
        {
            for(int i = 0; i < conns.Length; i ++)
            {
                Conn conn = conns[i];
                if (conn == null)
                    continue;
                if (!conn.isUse)
                    continue;
                lock(conn)
                {
                    conn.Close();
                }
            }
        }
    }
}
