/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/1/2017 6:25:50 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serv.core
{
    public class Conn
    {
        //常量
        public const int BUFFER_SIZE = 1024;
        //socket
        public Socket socket;
        //是否被使用
        public bool isUse = false;
        //buff
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public int buffCount = 0;
        //粘包分包
        public byte[] lenBytes = new byte[sizeof(UInt32)];
        public Int32 msgLength = 0;
        //心跳时间
        public long lastTickTime = long.MinValue;
        //对应的Player
        public Player player;

        ///构造函数
        public Conn()
        {
            readBuff = new byte[BUFFER_SIZE];
        }
        
        //初始化
        public void Init(Socket socket)
        {
            this.socket = socket;
            isUse = true;
            buffCount = 0;
            //心跳处理，稍后实现GetTimeStamp方法
            lastTickTime = Sys.GetTimeStamp();

        }

        //剩余的buff
        public int BuffRemain()
        {
            return BUFFER_SIZE - buffCount;
        }

        //获取客户端地址
        public string GetAdress()
        {
            if (!isUse)
                return "无法获取地址";
            return socket.RemoteEndPoint.ToString();
        }

        //关闭
        public void Close()
        {
            if (!isUse)
                return;
            if(player != null)
            {
                //玩家退出处理，稍后实现
                //player.Logout();
                return;
            }
            Console.WriteLine("[断开连接]" + GetAdress());
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            isUse = false;
        }

        //发送协议，相关内容稍后实现
        public void Send(ProtocolBase protocol)
        {
            ServNet.instance.Send(this, protocol);
        }
    }
}
