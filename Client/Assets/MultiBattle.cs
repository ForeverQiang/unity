using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBattle : MonoBehaviour {

    //单例
    public static MultiBattle instance;
    //坦克预设
    public GameObject[] tankPrefabs;
    //站产中的所有坦克
    public Dictionary<string, BattleTank> list = new Dictionary<string, BattleTank>();

    void Start()
    {
        //单例模式
        instance = this;
    }

    // 获取阵营 0 表示错误
    public int GetCamp(GameObject tankObj)
    {
        foreach(BattleTank mt in list.Values)
        {
            if (mt.tank.gameObhect == tankObj)
                return mt.camp;
        }
        return 0;
    }

    //是否同一阵营
    public bool IsSameCamp(GameObject tank1, GameObject tank2)
    {
        return GetCamp(tank1) == GetCamp(tank2);
    }

    //清理战场
    public void ClearBattle()
    {
        list.Clear();
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < tanks.Length; i++)
            Destroy(tanks[i]);
    }

    //开始战斗
    public void StartBattle(ProtocolBytes proto)
    {
        //解析协议
        int start = 0;
        string protooName = proto.GetString(start, ref start);
        if (protooName != "Fight")
            return;
        //坦克总数
        int count = proto.GetInt(start, ref start);
        //清理战场
        ClearBattle();
        //每一辆坦克
        for(int i = 0; i < count; i ++)
        {
            string id = proto.GetString(start, ref start);
            int team = proto.GetInt(start, ref start);
            int swopID == proto.GetInt(start, ref start);
            GenerateTank(id, team, swopID);
        }
        NetMgr.srvConn.msgDist.AddListener("UpdateUnitInfo", RecvUpdateUnitInfo);
        NetMgr.srvConn.msgDist.AddListener("Shooting", RecvShooting);
        NetMgr.srvConn.msgDist.AddListener("Hit", RecvHit);
        NetMgr.srvConn.msgDist.AddListener("Result", RecvResult);
    }

    //产生坦克
    public void GemerateTank(string id,int team, int swopID)
    {
        //获取出生点
        Transform sp = GameObject.Find("SwopPoints").transform;
        Transform swopTrans;
        if(team == 1)
        {
            Transform teampswop = sp.GetChild(0);
            swopTrans = teampswop.GetChild(swopID = 1);
        }
        else
        {
            Transform teamSwop = sp.GetChild(1);
            swopTrans = teamSwop.GetChild(swopID - 1);
        }
        if(swopTrans == null)
        {
            Debug.LogError("GenerateTank出生点错误 ");
            return;
        }

        //预设
        if(tankPrefabs.Length < 2)
        {
            Debug.LogError("坦克预设数量不够 ");
            return;
        }
        //产生坦克
        GameObject tankObj = (GameObject)instance(tankPrefabs[team - 1]);
        tankObj.name = id;
        tankObj.transform.position = swopTrans.position;
        tankObj.transform.rotation = swopTrans.rotation;
        //列表处理
        Battletank bt = new Battletank();
        bt.tank = tankObj.GetComponent<Tank>();
        bt.camp = team;
        list.Add(id, bt);
        //玩家处理
        if(id == GameMgr.instance.id)
        {
            bt.tank.ctrlType = tankObj.CtrlType.player;
            CameraFollow cf = Camera.main.gameObject.GetComponent<CameraFllow>();
            GameObject target = bt.tank.gameObject;
            cf.SetTarget(target);
        }
        else
        {
            //bt.tank.ctrlType = tankObj.CtrlType.net;
            //bt.tank.InitNetCtrl();//初始化网络同步
        }
    }

    public void RecvUpdateUnitInfo(ProtocolBase protocol)
    {
        //解析协议
        int start = 0;
        ProtocolBytes proto = (ProtocolBytes)protocol;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        Vector3 nPos;
        Vector3 nRot;
        nPos.x = proto.GetFloat(start, ref start);
        nPos.y = proto.GetFloat(start, ref start);
        nPos.z = proto.GetFloat(start, ref start);
        nRot.x = proto.GetFloat(start, ref start);
        nRot.y = proto.GetFloat(start, ref start);
        nRot.z = proto.GetFloat(start, ref start);
        float turretY = proto.GetFloat(start, ref start);
        float gunX = proto.GetFloat(start, ref start);
        //处理
        Debug.Log("[RecvUpdateUnitInfo " + id);
        if(!list.ContainsKey(id))
        {
            Debug.Log("RecvUpdateUnitInfo bt == null");
            return;
        }
        BattleTank bt = list(id);
        if (id == GameMgr.instance.id)
            return;
        bt.tank.NetForecastInfo(nPos, nRot);
        bt.tank.NetTurretTarget(turretY, gunX);
    }

    public void netTurretTarget(float y, float x)
    {
        turretRotTarget = y;
        turretTollTarget = x;
    }

    public void NetWheelsRotation()
    {
        float z = transform.InverseTransformPoint(fPos).z;
        //判断坦克是否正在移动
        if(Mathf.Abs(z)  < 0.1f || delta <= 0.05f)
        {
            motorAudioSource.Pause();
            return;
        }

        //轮子
        foreach(Transform wheel in wheels)
        {
            wheel.localEulerAngles += new Vector3(360 * z / delta, 0, 0);
        }

        //履带
        float offset = -wheels.GetChild(0).localEuler angles.x / 90f;
        foreach(Transform track in tracks)
        {
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Material mt1 = mr.material;
            mt1.mainTextureOffset = new Vector2(0, offset);
        }

        //声音
        if(!motorAudiSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }
    }
}
