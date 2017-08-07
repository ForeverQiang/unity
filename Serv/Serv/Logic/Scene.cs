/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 8/7/2017 4:44:30 PM
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
    public class Scene
    {
        //单例
        public static Scene instance;
        public Scene()
        {
            instance = this;
        }

        List<ScenePlayer> list = new List<ScenePlayer>();

        //根据名字获取ScenePlayer
        private ScenePlayer getScenePlayer (string id)
        {
            for(int i = 0; i < list.Count; i ++)
            {
                if (list[i].id == id)
                    return list[i];
            }

            return null;
        }

        //添加玩家
        public void AddPlayer(string id)
        {
            lock(list)
            {
                ScenePlayer p = new ScenePlayer();
                p.id = id;
                list.Add(p);
            }
        }

        //删除玩家
        public void DelPlayer(string id)
        {
            lock(list)
            {
                ScenePlayer p = getScenePlayer(id);
                if (p != null)
                    list.Remove(p);
            }

            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("PlayerLeave");
            protocol.AddString(id);
            ServNet.instance.Broadcast(protocol);
        }

        //发送列表
        public void SendPlayerList(Player player)
        {
            int count = list.Count;
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("GetList");
            protocol.AddString(count);
            protocol.AddInt(count);
            for(int i = 0; i < count; i ++ )
            {
                ScenePlayer p = list[i];
                protocol.AddString(p.id);
                protocol.AddFloat(p.x);
                protocol.AddFloat(p.y);
                protocol.AddFloat(p.z);
                protocol.AddInt(p.score);

            }
            player.Send(protocol);
        }


        //更新信息
        public void UpdateInfo(string id, float x, float y, float z, int score)
        {
            int count = list.Count;
            ProtocolBytes protocol = new ProtocolBytes();
            ScenePlayer p = getScenePlayer(id);
            if (p == null)
                return;
            p.x = x;
            p.y = y;
            p.z = z;
            p.score = score;
        }
    }
}
