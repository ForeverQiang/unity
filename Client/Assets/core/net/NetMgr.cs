/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/7/2017 1:46:09 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using UnityEngine;

public class NetMgr
{
    
    public void Update()
    {
        //消息
        msgdist.Update();
        //心跳
        if(Status ==  Status.Connected)
        {
            if(Time.time - lastTickTime > heartBeatTime)
            {
                ProtocolBase protocol = NetMgr.GetHeatBeatProtocol();
                send(protocol);
                lastTickTime = Time.time;
            }
        }
    }

    public static Connection SrvConn = new Connection();
    //public static Connection platformComm = new Connection()
    public static void Update()
    {
        SrvConn.Update();
        //platformComm.Update();
    }

    //心里
    public static ProtocolBase GetHeatBeatProtocol()
    {
        //具体的发送内容根据服务端设定进行改动
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HeatBeat");
        return protocol;
    }
}

