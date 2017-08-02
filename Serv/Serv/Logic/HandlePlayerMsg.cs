/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/2/2017 6:38:20 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using Serv.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serv.Logic
{
    public partial class HandlePlayerMsg
    {
        //获取分数
        //协议参数
        //返回协议： int分数
        public void MsgGetScore(Player player,ProtocolBase protoBase)
        {
            ProtocolBytes protocolRet = new ProtocolBytes();
            protocolRet.AddString("GetScore");
            protocolRet.AddInt(player.data.score);
            player.Send(protocolRet);
            Console.WriteLine("MsgGetScore " + player.id + player.data.score);
        }

        //增加分数
        //协议参数
        public void MsgAddScore(Player player,ProtocolBase protoBase)
        {
            //获取数值
            int start = 0;
            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            string protoName = protocol.GetString(start, ref start);
            //处理
            player.data.score += 1;
            Console.WriteLine("MsgAddScore " + player.id + " " + player.data.score.ToString());
        }
    }
}