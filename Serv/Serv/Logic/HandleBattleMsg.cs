/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/10/2017 1:19:50 PM
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

namespace Serv.Logic
{
    public class HandleBattleMsg
    {
        //开始战斗
        public void MsgStartFight(Player player, ProtocolBase protoBase)
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("StartFight");
            //条件判断
            if(player.tempData.status != PlayerTempData.Status.Room)
            {
                Console.WriteLine("MsgStartFight status err " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }

            if(!player.tempData.isOwner)
            {
                Console.WriteLine("MsgstartFight owner err " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }

            Room room = player.tempData.room;
            if(!room .CanStart())
            {
                Console.WriteLine("MsgStartFight CanStart err " + player.id)
                ;
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }

            //开始战斗
            protocol.AddInt(0);
            player.Send(protocol);
            room.StartFight();
        }


        //同步坦克单元
        public void MsgUpdateUnitInfo(Player player, ProtocolBase protoBase)
        {
            //获取数值
            int start = 0;
            ProtocolBytes protocol = (ProtocolBytes)protoBase;
            string protoName = protocol.GetString(start, ref start);
            float posX = protocol.GetFloat(start, ref start);
            float posY = protocol.GetFloat(start, ref start);
            float posZ = protocol.GetFloat(start, ref start);
            float rotX = protocol.GetFloat(start, ref start);
            float rotY = protocol.GetFloat(start, ref start);
            float rotZ = protocol.GetFloat(start, ref start);

            float gunRot = protocol.GetFloat(start, ref start);
            float gunRoll = protocol.GetFloat(start, ref start);
            //获取房间
            if (player.tempData.status != PlayerTempData.Status.Fight)
                return;
            Room room = player.tempData.room;
            //作弊校验 略
            player.tempData.posX = posX;
            player.tempData.PosY = posY;
            player.tempData.posZ = posZ;
            player.tempData.lastShootTime = Sys.GetTimeStamp();

            //广播
            ProtocolBytes protocolRet = new ProtocolBytes();
            protocolRet.AddString("UpdateUnitInfo");
            protocolRet.AddString(player.id);
            protocolRet.AddFloat(posX);
            protocolRet.AddFloat(posY);
            protocolRet.AddFloat(posZ);
            protocolRet.AddFloat(rotX);
            protocolRet.AddFloat(rotX);
            protocolRet.AddFloat(rotX);
            protocolRet.AddFloat(gunRot);
            protocolRet.AddFloat(gunRoll);

            room.Broadcast(protocolRet);

        }

    }
}
