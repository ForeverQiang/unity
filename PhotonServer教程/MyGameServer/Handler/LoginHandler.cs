/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/25/2017 5:32:37 PM
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
using MyGameServer.Manager;

 
namespace MyGameServer.Handler 
{
    class LoginHandler : BaseHandler
    {

        public LoginHandler()
        {
            OpCode = OperationCode.Login;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;
            UserManger manager = new UserManger();
            bool isSuccess =  manager.VerifyUser(username, password);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
           // response.ReturnCode = new OperationResponse(operationRequest.OperationCode);
            if(isSuccess)
            { 
                response.ReturnCode = (short)Common.ReturnCode.Success;
                peer.username = username;
            }
            else
            {
                response.ReturnCode = (short)Common.ReturnCode.Failed;  
            }
            peer.SendOperationResponse(response,sendParameters);
        }
    }
}
