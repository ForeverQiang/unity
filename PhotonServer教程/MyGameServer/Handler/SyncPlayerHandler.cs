/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/27/2017 3:10:24 PM
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
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;

namespace MyGameServer.Handler
{
    class SyncPlayerHandler : BaseHandler
    {
        public SyncPlayerHandler()
        {
            OpCode = OperationCode.SyncPlayer;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //取得所有已经登陆（在线玩家）的用户名
            List<string> usernameList = new List<string>();
            foreach (ClientPeer temppeer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(temppeer.username) == false &&
                    temppeer != peer)
                {
                    usernameList.Add(temppeer.username);
                }
            }

            StringWriter sw = new StringWriter();
            XmlSerializer serizlizer = new XmlSerializer(typeof(List<string>));
            serizlizer.Serialize(sw, usernameList);
            sw.Close();
            string usernameListString = sw.ToString();


            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.UsernameList, usernameListString);
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = data;
            peer.SendOperationResponse(response, sendParameters);

            //告诉其他客户端有新的客户端加入
            foreach (ClientPeer tempeer in MyGameServer.Instance.peerList)
            {
                if(string.IsNullOrEmpty(tempeer.username) ==false && tempeer !=peer)
                {
                    EventData ed = new EventData((byte)EventCode.NewPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();

                    data2.Add((byte)ParameterCode.Username, peer.username);
                    ed.Parameters = data2;
                    tempeer.SendEvent(ed, sendParameters);
                }
            }
        }
    }
}
