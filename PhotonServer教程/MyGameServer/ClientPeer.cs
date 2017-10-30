/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/20/2017 5:04:09 PM
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
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Handler;
using Common.Tools;
using Common;

namespace MyGameServer
{
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {

        public float x, y, z;
        public string username;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }
        //断开客户端断开连接的后续工作
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyGameServer.Instance.peerList.Remove(this);
        }
        //处理客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {

            BaseHandler handler = DictTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDic,(OperationCode)operationRequest.OperationCode);

            if(handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                BaseHandler defaultHandler =  DictTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDic, OperationCode.Default  );
                defaultHandler.OnOperationRequest(operationRequest, sendParameters, this);

            }
            //switch(operationRequest.OperationCode)//通过OperationCode 区分请求 
            // {
            //     case 1:
            //         MyGameServer.log.Info("收到一个客户端的请求");
            //         Dictionary<byte, object> data = operationRequest.Parameters;
            //         object intValue;
            //         data.TryGetValue(1, out intValue);
            //         object stringValue;
            //         data.TryGetValue(2, out stringValue);
            //         MyGameServer.log.Info("得到的参数数据是：" + intValue.ToString() + stringValue.ToString());

            //         OperationResponse opResponse = new OperationResponse(1);
            //         Dictionary<byte, object> data2 = new Dictionary<byte, object>();
            //         data2.Add(1, 100);
            //         data2.Add(2, "errr23141撒谎地方");
            //         opResponse.SetParameters(data2);
            //         SendOperationResponse(opResponse, sendParameters);//给客户端一个响应
            //         EventData ed = new EventData(1);
            //         ed.Parameters = data2;

            //         SendEvent(ed, new SendParameters());
            //         break;
            //     case 2:
            //         break;
            //     default:
            //         break;
            // }
        }
    }
}
