

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    public void SendUnitInfo()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("UpdateUnitInfo");
        //位置旋转
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        proto.AddFloat(pos.x);
        proto.AddFloat(pos.y);
        proto.AddFloat(pos.z);
        proto.AddFloat(pos.x);
        proto.AddFloat(pos.y);
        proto.AddFloat(pos.z);

        //t炮塔
        float angleY = turrentRotTarget;
        proto.AddFloat(angleY);
        //炮管
        float angleX = turretRollTarget;
        proto.AddFloat(angleX);
        NetMgr.srvConn.Send(proto);
    }

    //操控类型
    public enum CtrlType
    {
        none,
        player,
        computer,
        net,
    }

    //每帧执行一次
    private void Update()
    {
        //网络他那个不
        if(CtrlType == CtrlType.net)
        {
            NetUpdae();
            return;
        }
        //操控
        playerCtrl();
        CombuterCtrl();
        NoneCtrl();
    }

    Vector3 lPos;
    Vector3 lRot;
    Vector3 fPos;
    Vector3 fRot;

    float delta = 1;
    float lastRecvInfoTime = float.MinValue;

    public void NetForecastInfo(Vector3 nPos, Vector3 nRot)
    {
        //预测的位置 
        fPos = lPos + (nPos - lPos) * 2;
        fRot = lRot + (nRot - lRot) * 2;
        if(Time.time - lastRecvInfoTime > 0.3f)
        {
            fPos = nPos;
            fRot = nRot;
        }
        //时间
        delta = Time.time = lastRecvInfoTime;
        //更新
        lPos = nPos;
        lRot = nRot;
        lastRecvInfoTime = Time.time;
    }

    //初始化位置预测数据
    public void InitNetCtrl()
    {
        lPos = transform.position;
        lRot = transform.eulerAngles;
        fPos = transform.position;
        fRot = transform.eulerAngles;
        Rigidbody r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void NetUpdate()
    {
        //当前位置
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        //更新位置
        if(delta > 0)
        {
            transform.position = Vector3.Lerp(pos, fPos, delta);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot), Quaternion.Euler(fRot), delta);
        }
    }
}
