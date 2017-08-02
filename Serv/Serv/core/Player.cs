/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 7/31/2017 4:01:08 PM
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

namespace Serv
{
    public class Player
    {
        //id、连接、玩家数据
        public string id;
        public Conn conn;
        public PlayerData data;
        public PlayerTempData tempData;


        //构造函数，给id和conn赋值
        public Player(string id, Conn conn)
        {
            this.id = id;
            this.conn = conn;
            tempData = new PlayerTempData();
        }

        //发送
        public void Send(ProtocolBase proto)
        {
            if (conn == null)
                return;
            ServNet.instance.Send(conn, proto);
        }

        //踢下线
        public static bool KickOff(string id, ProtocolBase proto)
        {
            Conn[] conns = ServNet.instance.conns;
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)
                    continue;
                if (!conns[i].isUse)
                    continue;
                if (conns[i].player == null)
                    continue;
                if (conns[i].player.id == id)
                {
                    lock (conns[i].player)
                    {
                        if (proto != null)
                            conns[i].player.Send(proto);

                        return conns[i].player.Logout();
                    }

                }
            }
            return true;
        }

        //下线
        public bool Logout()
        {
            //事件处理、
            //ServNet.instance.handlePlayerEvent.OnLogout(this);
            //保存
            if (!DataMgr.instance.SavePlayer(this))
                return false;
            //下线
            conn.player = null;
            conn.Close();
            return true;
        }
    }
}
