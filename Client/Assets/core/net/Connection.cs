 /**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/4/2017 1:16:46 PM
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
using UnityEngine;

public class Connection: MonoBehaviour
{
    //常量
    const int BUFFER_SIZE = 1024;
    //socket
    private Socket socket;
    //buff
    private byte[] readBuff = new byte[BUFFER_SIZE];
    private int buffCount = 0;
    //粘包分包
    private Int32 msgLength = 0;
    private byte[] lenBytes = new byte[sizeof(Int32)];
    //协议
    public ProtocolBase proto;
    //心跳时间
    public float lastTickTime = 0;
    public float heartBeatTime = 30;
    //消息分发
    public MsgDistribution msgDist = new MsgDistribution();
    //状态
    public enum Status
    {
        None,
        Connected,
    };
    public Status status = Status.None;


    //连接服务器
    public bool Connect(string str,int port)
    {
        try
        {
            //socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //connect
            socket.Connect(host, port);
            //BeginReceive
            socket.BeginReceive(readBuff, buffCount, BUFFER_SIZE - buffCount, SocketFlags.None, ReceiveCb, readBuff);
            //状态
            status = Status.Connected;
            return true;
        }
        catch(Exception e)
        {
            Debug.Log("连接失败： " + e.Message);
            return false;
        }
    }

    //关闭连接
    public bool Close()
    {
        try
        {
            socket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("关闭连接: " + e.Message);
            return false;
        }
    }

    //接收回调
    public void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            buffCount = buffCount + count;
            ProcessData();
            socket.BeginReceive(readBuff, buffCount, BUFFER_SIZE - buffCount, SocketFlags.None, ReceiveCb, readBuff);
        }
        catch(Exception e)
        {
            Debug.Log("ReceiveCb 失败 :" + e.Message);
            status = Status.None;
        }
    }

    //消息处理
    private void ProcessData()
    {
        //粘包分包处理
        if (buffCount < sizeof(Int32))
            return;

        //包体长度
        Array.Copy(readBuff, lenBytes, sizeof(Int32));
        msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(Int32))
            return;
        //协议编码
        ProtocolBase protocol = proto.Decode(readBuff, sizeof(Int32), msgLength);
        Debug.Log("收到信息:" + protocol.GetDesc());
        lock(msgDist.msgList)
        {
            msgDist.msgList.Add(protocol);
        }
        //清除已处理的消息
        int count = buffCount - msgLength - sizeof(Int32);
        Array.Copy(readBuff, sizeof(Int32) + msgLength, readBuff, 0, count);
        buffCount = count;
        if(buffCount > 0 )
        {
            ProcessData();
        }
    }

    public bool Send(ProtocolBase protocol)
    {
        if(status != Status.Connected)
        {
            Debug.Log("[Connection] 还没有连接就发送数据是不好的");
            return true;
        }

        byte[] b = protocol.Encode();
        byte[] length = BitConverter.GetBytes(b.Length);

        byte[] sendbuff = length.Concat(b).ToArray();
        socket.Send(sendbuff);
        Debug.Log("[发送消息] " + protocol.GetDesc());
        return true;
    }

    public bool Send(ProtocolBase protocol, string cbName, MsgDistribution.Delegate cb)
    {
        if (status != Status.Connected)
            return false;
        msgDist.AddOnceListener(cbName, cb);
        return Send(protocol);
    }

    public bool Send(ProtocolBase protocol, MsgDistribution.Delegate cb )
    {
        string cbName = protocol.GetName();
        return Send(protocol, cbName, cb);
    }
}

