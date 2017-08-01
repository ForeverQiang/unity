/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 7/31/2017 4:04:31 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serv
{
    public class DataMgr
    {
        MySqlConnection sqlconn;

        //单例模式
        public static DataMgr instance;

        public object MemeoryStream { get; private set; }

        public DataMgr()
        {
            instance = this;
            Connect();
        }

        //链接
        public void Connect()
        {
            // string connStr = "Database=game,Data Source=127.0.0.1;";
            // connStr += "User ID=root;Password=wujunqiang;port=3306";
            string connStr = "Server=127.0.0.1;Database=game;Uid=root;Pwd=wujunqiang;CharSet=utf8;";
            sqlconn = new MySqlConnection(connStr);
            try
            {
                sqlconn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgt]Connect" + e.Message);
                return;
            }
        }

        //判断安全字符串
        public bool IsSafeStr(string str)
        {
            //Console.WriteLine(!Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']"));
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        //是否存在该用户
        private bool CanRegister(string id)
        {
            if (!IsSafeStr(id))
            {
                Console.WriteLine(IsSafeStr(id));
                return false;
            }
                

            //查询id是否存在
            string cmdStr = string.Format("select * from user where id='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);

            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return !hasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("[DataMgr]CanRegister fail" + e.Message);
                return false;
            }
        }

        //注册
        public bool Register(string id,string pw)
        {
            //防止sql注入
            if (!IsSafeStr(id) || !IsSafeStr(pw))
            {
                Console.WriteLine("[DataMgr]Register 使用非法字符");
                return false;
            }
            //能否注册
            if (!CanRegister(id))
            {
                Console.WriteLine("[DataMgr]Register !CanRegister");
                return false;
            }
            //写入数据库user表
            string cmdStr = string.Format("insert into user set id = '{0}', pw = '{1}';", id, pw);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]Register " + e.Message);
                return false;
            }
        }

        //创建角色
        public bool CreatePlayer(string id)
        {
            //防止sql注入
            if (!IsSafeStr(id))
                return false;

            //序列化
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            PlayerData playerData = new PlayerData();
            try
            {
                formatter.Serialize(stream, playerData);
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]CreatePlayer 序列化" + e.Message);
                return false;
            }
            byte[] byteArr = stream.ToArray();
            
            //写入数据库
            string cmdStr = string.Format("insert into player set id='{0}', data = @data;", id);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);
            cmd.Parameters.Add("@data", MySqlDbType.Blob);
            cmd.Parameters[0].Value = byteArr;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)  
            {
                Console.WriteLine("[DataMgr]CreatePlayer 写入" + e.Message);
                return false;
            }
        }

        //检测用户名和密码
        public bool CheckPassWord(string id,string pw)
        {
            //防止sql注入
            if (!IsSafeStr(id))
                return false;
            // 查询
            string cmdStr = string.Format("select  * from user where id='{0}' and pw='{1}';", id, pw);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);
            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return hasRows;
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]CheckPassWord " + e.Message);
                return false;
            }
        }

        //获取玩家数据
        public PlayerData GetPlayerData(string id)
        {
            PlayerData playerdata = null;
            //防止sql注入
            if (!IsSafeStr(id))
                return playerdata;
            //查询
            string cmdStr = string.Format("select * from player where id = '{0}';", id);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);
            byte[] buffer = new byte[1];
            try
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if(!dataReader.HasRows)
                {
                    dataReader.Close();
                    return playerdata;
                }
                dataReader.Read();

                long len = dataReader.GetBytes(1, 0, null, 0, 0);//1是data
                buffer = new byte[len];
                dataReader.GetBytes(1, 0, buffer, 0, (int)len);
                dataReader.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]GetPlayerData 查询 " + e.Message);
                return playerdata;
            }

            //反序列化
            MemoryStream stream = new MemoryStream(buffer);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                playerdata = (PlayerData)formatter.Deserialize(stream);
                return playerdata;
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]GetPlayData 反序列化 " + e.Message);
                return playerdata;
            }
        }

        //保存数据
        public bool SavePlayer(Player player)
        {
            string id = player.id;
            PlayerData playerdata = player.data;

            ///序列化
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            try
            {
                formatter.Serialize(stream, playerdata);
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]SavePlayer 序列化  " + e.Message);
                return false;
            }

            byte[] byteArr = stream.ToArray();

            //写入数据库
            string formatStr = "Update player set data=@data where id='{0}';";
            string cmdStr = string.Format(formatStr, player.id);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlconn);
            cmd.Parameters.Add("@data", MySqlDbType.Blob);
            cmd.Parameters[0].Value = byteArr;
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("[DataMgr]SavePlayer 写入" + e.Message);
                return false;
            }
        }

    }
}
