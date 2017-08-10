/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/9/2017 6:01:55 PM
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
    public class RoomMgr
    {
        //单例
        public static RoomMgr instance;
        public RoomMgr()
        {
            instance = this;
        }
        //房间列表
        public List<Room> list = new List<Room>();

        //创建房间
        public void CreateRoom(Player player)
        {
            Room room = new Room();
            lock(list)
            {
                list.Add(room);
                room.AddPlayer(player);
            }
        }

        //玩家离开
        public void LeaveRoom(Player player)
        {
            PlayerTempData tempData = player.tempData;
            if (tempData.status == PlayerTempData.Status.None)
                return;
            Room room = tempData.room;
            lock(list)
            {
                room.DelPlayer(player.id);
                if (room.list.Count == 0)
                    list.Remove(room);
            }
        }

        //列表
        public ProtocolBytes GetRoomList()
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("GetRoomList");
            int count = list.Count;
            //房间数量
            protocol.AddInt(count);
            //每个房间信息
            for(int i = 0; i < count; i ++)
            {
                Room room = list[i];
                protocol.AddInt(room.list.Count);
                protocol.AddInt((int)room.status);
            }
            return protocol;
        }

        //创建房间
        public void MsgCreateRoom(Player player, ProtocolBase protoBase)
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("CreateRoom");
            //条件检测
            if(player.tempData.status != PlayerTempData.Status.None)
            {
                Console.WriteLine("MsgCreateRoom Fail " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }
            RoomMgr.instance.CreateRoom(player);
            protocol.AddInt(0);
            player.Send(protocol);
            Console.WriteLine("MsgCreateRoom OK" + player.id);
        }

        //加入房间
        public void MsgEnterRoom(Player player, ProtocolBase protocolBase)
        {
            //获取数据
            int start = 0;
            ProtocolBytes protocol = (ProtocolBytes)protocolBase;
            string protoName = protocol.GetString(start, ref start);
            int index = protocol.GetInt(start, ref start);
            Console.WriteLinen("[收到MsgEnterRoom]" + player.id + " " + index);

            //
            protocol = new ProtocolBytes();
            protocol.AddString("EnterRoom");
            //判断房间是否存在
            if(index < 0 || index >= RoomMgr.instance.list.Count)
            {
                Console.WriteLine("MsgEnterRoom Indexerr " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }

            Room room = RoomMgr.instance.list[index];
            //判断房间的状态
            if(room.status != Room.Status.Prepare)
            {
                Console.WriteLine("MsgEnterRoom status err " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }
            //添加玩家
            if(room.AddPlayer(player))
            {
                room.Broadcast(room.GetRoomInfo());
                protocol.AddInt(0);
                player.Send(protocol);
            }
            else
            {
                Console.WriteLine("MsgEnterRoom maxplayer err " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
            }
        }

        //获取房间信息
        public void MsgGetRoomInfo (Player player, ProtocolBase protoBase)
        {
            if(player.tempData.status != PlayerTempData.Status.Room)
            {
                Console.WriteLine("MsgGetRoomInfo status err " + player.id);
                return;
            }
            Room room = player.tempData.room;
            player.Send(room.GetRoomInfo());
        }

        // 离开房间
        public void msgLeaveRoom(Player player, ProtocolBase protoBase)
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("LeaveRoom");
            //条件检测
            if(player.tempData.status != PlayerTempData.Status.Room)
            {
                Console.WriteLine("MsgLeaveRoom Statuss err " + player.id);
                protocol.AddInt(-1);
                player.Send(protocol);
                return;
            }
            //处理
            protocol.AddInt(0);
            player.Send(protocol);
            Room room = player.tempData.room;
            RoomMgr.instance.LeaveRoom(player);
            //广播
            if(room!=null)
            {
                room.Broadcast(room.GetRoomInfo());
            }
        }
    }

}
