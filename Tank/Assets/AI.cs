using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    //所控制的坦克
    public Tank tank;
    //路径
    private Path path = new Path();

    //初始化路点
    void InitWaypoint()
    {
        GameObject obj = GameObject.Find("WaypointContainer");
        if (obj && obj.transform.GetChild(0 ) != null)
        {
            Vector3 targetPos = obj.transform.GetChild(0).position;
            path.InitByNavMeshPath(transform.position, targetPos);
        }
    }



    //状态枚举
    public enum Status
    {
        Patrol,//巡逻
        Attack,//攻击
    }

    private Status status = Status.Patrol;//默认巡逻

    //更改状态
    public void ChangeStatus(Status status)
    {
        if (status == Status.Patrol)
            PatrolStart();
        else if (status == Status.Attack)
            AttackStart();
    }
    void Start()
    {
        InitWaypoint();
    }

    private void Update()
    {
        if (tank.ctrlType != Tank.CtrlType.computer)
            return;
        if (status == Status.Patrol)
            PatrolUpdate();
        else if (status == Status.Attack)
            AttackUpdate();

        TargetUpdate();
        //行走
        if(path.IsReach(transform))
        {
            path.NextWaypoint();
        }
    }

    /// <summary>
    /// 巡逻开始
    /// </summary>
    void PatrolStart()
    {

    }

    /// <summary>
    /// 攻击开始
    /// </summary>
    void AttackStart()
    {

        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }

    //上次更新路径时间
    private float lastUpdateWaypointTime = float.MinValue;
    //更新路径cd
    private float updateWaypointtInterval = 10;
    /// <summary>
    /// 巡逻中
    /// </summary>
    void PatrolUpdate()
    {
        //发现敌人
        if (target != null)
            ChangeStatus(Status.Attack);
        //时间间隔
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointtInterval)
            return;
        lastUpdateWaypointTime = Time.time;
        //处理巡逻点
        if(path.waypoints == null|| path.isFinish)
        {
            GameObject obj = GameObject.Find("WaypointContainer");
            {
                int count = obj.transform.childCount;
                if (count == 0)
                    return;
                int index = Random.Range(0, count);
                Vector3 targetpos = obj.transform.GetChild(index).position;
                path.InitByNavMeshPath(transform.position, targetpos);
            }
        }
    }

    /// <summary>
    /// 攻击中
    /// </summary>
    void AttackUpdate()
    {
        //目标丢失
        if (target == null)
            ChangeStatus(Status.Patrol);
        //时间间隔
        float interval = Time.time - lastUpdateWaypointTime;
        if (interval < updateWaypointtInterval)
            return;
        //更新路径
        Vector3 targetpos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetpos);
    }

    //锁定的坦克
    private GameObject target;
    //视野范围
    private float sightDistance = 30;
    //上一次搜寻时间
    private float lastSearchTargetTime = 0;
    //搜寻间隔
    private float searchTargetInterval = 3;
    /// <summary>
    /// 搜寻目标
    /// </summary>
    void TargetUpdate()
    {
        //cd时间
        float interval = Time.time - lastSearchTargetTime;
        if (interval < searchTargetInterval)
            return;
        lastSearchTargetTime = Time.time;

        //已有目标的情况，判断是否丢失目标
        if (target != null) 
            HasTarget();
        else
            NoTarget();
    }
    /// <summary>
    /// 已有目标的情况，判断是否丢失目标
    /// </summary>
    void HasTarget()
    {
        Tank targetTank = target.GetComponent<Tank>();
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;

        if(targetTank.ctrlType == Tank.CtrlType.none)
        {
            Debug.Log("目标死亡，丢失目标");
            target = null;  
        }
        else if(Vector3.Distance(pos,targetPos) > sightDistance)
        {
            Debug.Log("目标过远，丢失目标");
            target = null;
        }
    }
    /// <summary>
    /// 没有目标的情况，搜索视野中的坦克
    /// </summary>
    void NoTarget()
    {
        //最小生命值
        float minHp = float.MaxValue;
        //遍历所有坦克
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Tank");
        for(int i = 0; i < targets.Length; i++)
        {
            //Tank组件
            Tank tank = targets[i].GetComponent<Tank>();
            if(tank == null)
            {
                continue;
            }
            //自己
            if (targets[i] = gameObject)
                continue;
            //死亡
            if (tank.ctrlType == Tank.CtrlType.none)
                continue;
            //判断距离
            Vector3 pos = transform.position;
            Vector3 targetPos = targets[i].transform.position;
            if (Vector3.Distance(pos, targetPos) > sightDistance)
                continue;
            //判断生命
            if (minHp > tank.hp)
                target = tank.gameObject;
        }
        //调试
        if (target != null)
            Debug.Log("获取目标" + target.name);
    }


    /// <summary>
    ///     //被攻击
    /// </summary>
    /// <param name="attackTank"></param>
    public void OnAttecked(GameObject attackTank)
    {
        target = attackTank;
    }

    /// <summary>
    /// 获取炮管和炮塔的目标角度
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTurretTarget()
    {
        //没有目标，朝着则炮塔坦克前方
        if(target == null)
        {
            float y = transform.eulerAngles.y;
            Vector3 rot = new Vector3(0, y, 0);
            return rot;
        }
        //有目标，则对准目标
        else
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 vec = targetPos - pos;
            return Quaternion.LookRotation(vec).eulerAngles;
        }
    }


    public bool IsShoot()
    {
        if (target == null)
            return false;
        //目标角度差
        float turretRoll = tank.turret.eulerAngles.y;
        float angle = turretRoll = GetTurretTarget().y;
        if (angle < 0)
            angle += 360;
        //30度以内发射炮弹
        if (angle < 30 || angle > 330)
            return true;
        else
            return false;
    }

    /// <summary>
    //获取转向角
    /// </summary>
    /// <returns></returns>
    public float GetSteering()
    {
        if (tank == null)
            return 0;
        Vector3 itp = transform.InverseTransformPoint(path.waypoint);
        if (itp.x > path.deviation / 5)
            return tank.maxSteeringAngle;
        else if (itp.x < -path.deviation / 5)
            return -tank.maxSteeringAngle;
        else
            return 0;
    }

    /// <summary>
    /// 获取马力
    /// </summary>
    /// <returns></returns>
    public float GetMotor()
    {
        if (tank == null)
            return 0;
        Vector3 itp = transform.InverseTransformPoint(path.waypoint);
        float x = itp.x;
        float z = itp.z;
        float r = 6;

        if (z < 0 && Mathf.Abs(x) < -z && Mathf.Abs(x) < r)
            return -tank.MaxMotorTorque;
        else
            return tank.maxBrakeTorque;
    }
   
    /// <summary>
    /// 获取刹车
    /// </summary>
    /// <returns></returns>
    public float GetBrakeTorque()
    {
        if (path.isFinish)
            return tank.MaxMotorTorque;
        else
            return 0;
    }

    private void OnDrawGizmos()
    {
        path.DrawWaypoints();
    }

  

}
