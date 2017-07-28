﻿/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 7/29/2017 12:32:51 AM
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

namespace Serv
{
    class Conn
    {
        //常量
        public const int BUFFER_SIZE = 1024;
        //socket
        public Socket socket;
        //是否使用
        public bool isUse = false;
        //buff
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public int buffCount = 0;
        //构造函数
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
        }
        //缓冲区剩余的字节数
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
            Console.WriteLine("[断开连接]" + GetAdress());
            socket.Close();
            isUse = false;
        }
    }
}
