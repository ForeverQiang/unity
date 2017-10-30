/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/27/2017 1:46:13 PM
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
using Common.Tools;

namespace MyGameServer.Handler
{
    class SyncPositionHandler : BaseHandler
    {
        public SyncPositionHandler()
        {
            OpCode = OperationCode.SyncPosition;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            // Vector3Data pos = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Position) as Vector3Data;
            float x = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.X);
            float y = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Y);
            float z = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Z);

            MyGameServer.log.Info(x + " " + y + " " + z);
            peer.x = x;
            peer.y = y;
            peer.z = z;

            MyGameServer.log.Info("X: " + x + "Y: " + y + "Z: " + z);
        }
    }
}
