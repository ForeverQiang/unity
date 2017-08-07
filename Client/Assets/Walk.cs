using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Walk : MonoBehaviour
{

    //socker 和缓冲区
    Socket socket;
    const int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    //玩家列表
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    //self
    string palyerID = "";
    //上次移动的时间
    public float lastMoveTime;

    //消息列表
    List<string> msgList = new List<string>();
    //Player预设
    public GameObject prefab;
    //自己的IP和端口
    string id;
    //粘包
    int buffCount = 0;
    byte[] lenBytes = new byte[sizeof(UInt32)];
    Int32 msgLength = 0;

    public static Walk instance;

    void Start()
    {
        instance = this;
    }
    //添加玩家
    void AddPlayer(string id, Vector3 pos)
    {
        GameObject player = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        TextMesh textMesh = player.GetComponent<TextMesh>();
        textMesh.text = id + ":" + score; 
        players.Add(id, player);
    }

    //删除玩家
    void DelPlayer(string id)
    {
        //已经初始化该玩家
        if(players.ContainsKey(id))
        {
            Destroy(players[id]);
            players.Remove(id);
        }
    }

    //更新分数
    public void UpdateScore(string id, int score)
    {
        GameObject player = players[id];
        if (player == null)
            return;
        TextMesh textmesh = player.GetComponent<TextMesh>();
        textmesh.text = id + ": " + score;
    }

    //更新信息
    public void UpdateInfo(string id, Vector3 pos, int score)
    {
        //只更新自己的分数
        if(id == playerID)
        {
            UpdateScore(id, score);
            return;
        }

        //其他人
        //已经初始化该玩家
        if(players.ContainsKey(id))
        {
            players[id].transform.position = pos;
            UpdateScore(id, score);
        }


        //尚未初始化该玩家
        else
        {
            AddPlayer(id, pos, score);
        }
    }

    public void startGame(string id)
    {
        playerID = id;
        //产生自己
        UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
        float x = 100 + UnityEngine.Random.Random(-30, 30);
        float y = 0;
        float z = 100 + UnityEngine.Random.Random(-30, 30);
        Vector3 pos = new Vector3(x, y, z);
        AddPlayer(playerID, pos, 0);
        //同步
        SendPos();

        //获取列表
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("GetList");
        NetMgr.SrvConn.Send(proto, GetList);
        NetMgr.SrvConn.msgDist.AddListener("UpdateInfo", UpdateInfo);
        NetMgr.SrvConn.msgDist.AddListener("PlayerLeave", PlayerLeave);

    }

    //发送位置协议
    void SendPos()
    {
        GameObject player = players[playerID];
        Vector3 pos = player.transform.position;
        //组装协议
        //string str = "POS ";
        //str += id + " ";
        //str += pos.x.ToString() + " ";
        //str += pos.y.ToString() + " ";
        //str += pos.z.ToString() + " ";

        //消息
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("UpdateInfo");
        proto.AddFloat(pos.x);
        proto.AddFloat(pos.y);
        proto.AddFloat(pos.z);
        NetMgr.SrvConn.Send(proto);

        //byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
        //socket.Send(bytes);
        //Debug.Log("发送" + str);
    }

    //更新列表
    public void GetList(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        //获取头部数值
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int count = proto.GetInt(start, ref start);

        //遍历
        for(int i = 0; i < count; i ++)
        {
            string id = proto.GetString(start, ref start);
            float x = proto.GetFloat(start, ref start);
            float y = proto.GetFloat(start, ref start);
            float z = proto.GetFloat(start, ref start);
            int Score = proto.GetInt(start, ref start);
            Vector3 pos = = new Vector3(x, y, z);
            UpdateInfo(id, pos, score);
        }
    }

    //更新信息
    public void UpdateInfo(ProtocolBase protocol)
    {
        //获取数值
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        float x = proto.GetFloat(start, ref start);
        float y = proto.GetFloat(start, ref start);
        float z = proto.GetFloat(start, ref start);
        int score = proto.GetInt(start, ref start);
        Vector3 pos = new Vector3(x, y, z);
        UpdateInfo(id, pos, score);
    }

    //玩家离开
    public void PlayerLeave(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        //获取数值
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);

        DelPlayer(id);
    }


    //发送离开协议
    void SendLeave()
    {
        //组装协议
        string str = "LEAVE ";
        str += id + " ";
        byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
        socket.Send(bytes);
        Debug.Log("发送 " + str);
    }

    //移动
    void Move()
    {
        if (id == "")
            return;

        GameObject player = players[id];
        //上
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position += new Vector3(0, 0, 1);
            SendPos();
        }
        //下
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.position += new Vector3(0, 0, -1);
            SendPos();
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.position += new Vector3(-1, 0, 0);
            SendPos();
        }
        //右
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.position += new Vector3(1, 0, 0);
            SendPos();
        }
    }

    //离开
    private void OnDestroy()
    {
        SendLeave();
    }

    //开始
    void Start()
    {
        Connect();
        //把自己放在一个随机位置
        UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
        float x = 100 + UnityEngine.Random.Range(-30, 30);
        float y = 0;
        float z = 100 + UnityEngine.Random.Range(-30, 30);
        Vector3 pos = new Vector3(x, y, z);
        AddPlayer(id, pos);
        //同步
        SendPos();
    }

    //链接
    void Connect()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connect
        socket.Connect("127.0.0.1", 1234);
        id = socket.LocalEndPoint.ToString();
        //Recv
        socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    //接收回掉
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            //数据处理
            //  string str = System.Text.Encoding.UTF8.GetString(readBuff, 0, count);
            //  msgList.Add(str);

            buffCount += count;
            ProcessData();
            //继续接受
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }
        catch (Exception e)
        {
            socket.Close();
        }
    }

    private void ProcessData()
    {
        //小于长度字节
        if (buffCount < sizeof(Int32))
            return;
        //消息长度
        Array.Copy(readBuff, lenBytes, sizeof(Int32));
        msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(Int32))
            return;
        //处理消息
        string str = System.Text.Encoding.UTF8.GetString(readBuff, sizeof(Int32), (int)msgLength);
      //  recvStr = str;
        //清除已处理的消息
        int count = buffCount - msgLength - sizeof(Int32);
        Array.Copy(readBuff, msgLength, readBuff, 0, count);
        buffCount = count;
        if(buffCount > 0)
        {
            ProcessData();
        }

    }

    void Update()
    {
        //处理消息列表上
        for (int i = 0; i < msgList.Count; i++)
            HandleMsg();
        //移动
        Move();
    }

    //处理消息列表
    void HandleMsg()
    {
        //获取一条消息
        if (msgList.Count <= 0)
            return;

        string str = msgList[0];
        msgList.RemoveAt(0);
        //根据协议做不同的处理
        string[] args = str.Split(' ');
        if (args[0] == "POS")
        {
            OnRecvPos(args[1], args[2], args[3], args[4]);
        }
        else if (args[0] == "LEAVE")
        {
            OnRecvLeave(args[1]);
        }
    }

    //处理更新位置的协议
    public void OnRecvPos(string id, string xStr, string yStr, string zStr)
    {
        //不更新自己的位置
        if (id == this.id)
            return;
        //解析协议
        float x = float.Parse(xStr);
        float y = float.Parse(yStr);
        float z = float.Parse(zStr);
        Vector3 pos = new Vector3(x, y, z);
        //已经初始化该玩家
        if (players.ContainsKey(id))
        {
            players[id].transform.position = pos;
        }
        //尚未初始化该玩家
        else
        {
            AddPlayer(id, pos);
        }
    }

    //处理玩家离开的协议
    public void OnRecvLeave(string id)
    {
        if (players.ContainsKey(id))
        {
            Destroy(players[id]);
            players[id] = null;
        }
    }
}
