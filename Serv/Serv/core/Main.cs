﻿using Serv.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serv
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            DataMgr dataMgr = new DataMgr();
            //注册
            bool ret = dataMgr.Register("Lpy", "123");
            if (ret)
                Console.WriteLine("注册成功");
            else
                Console.WriteLine("注册失败");

            //创建玩家
            ret = dataMgr.CreatePlayer("Lpy");
            if (ret)
                Console.WriteLine("创建玩家成功");
            else
                Console.WriteLine("创建玩家失败 ");

            //获取玩家数据
            PlayerData pd = dataMgr.GetPlayerData("Lpy");
            if (pd != null)
                Console.WriteLine("获取玩家成功 分数是 " + pd.score);
            else
                Console.WriteLine("获取玩家数据失败");
            //更改玩家数据
            pd.score += 10;
            //保存数据
            Player p = new Player();
            p.id = "Lpy";
            p.data = pd;
            dataMgr.SavePlayer(p);
            //重新获取
            pd = dataMgr.GetPlayerData("Lpy");
            if (pd != null)
                Console.WriteLine("获取玩家成功 分数是  " + pd.score);
            else
                Console.WriteLine("重新获取玩家数据失败");

            ServNet servNet = new ServNet();
            servNet.Start("127.0.0.1", 1234);
            Console.ReadLine();


        }
    }
}
