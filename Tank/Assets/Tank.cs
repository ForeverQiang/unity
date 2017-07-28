using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{

    //操纵类型
    public enum CtrlType
    {
        none,
        player,
        computer
    }
    public CtrlType ctrlType = CtrlType.player;

    //最大生命值
    private float maxHp = 100;
    //当前生命值
    public float hp = 100;

    //焚烧特效
    public GameObject DestoryEffect;

    //发射炮弹音源
    public AudioSource shootAudioSource;
    //发射音效
    public AudioClip shootClip;
    //人工智能
    private AI ai;
    // Use this for initialization
    void Start()
    {
        //获取炮塔
        turret = transform.Find("turret");
        //获取炮管
        gun = turret.Find("gun");
        //获取轮子
        wheels = transform.Find("wheels");
        //获取履带
        tracks = transform.Find("tranks");
        //马达音效
        motorAudioSource = gameObject.AddComponent<AudioSource>();
        motorAudioSource.spatialBlend = 1;
        //发射音源
        shootAudioSource = gameObject.AddComponent<AudioSource>();
        shootAudioSource.spatialBlend = 1;

        //人工智能
        if(ctrlType == CtrlType.computer)
        {
            ai = gameObject.AddComponent<AI>();
            ai.tank = this;
        }
    }

    // Update is called once per frame
    void Update()
    {


        //float speed = 1;

        //if(Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //    transform.position += transform.forward * speed;
        //}
        //else if(Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 180, 0);
        //    transform.position += transform.forward * speed;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 270, 0);
        //    transform.position += transform.forward * speed;
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 90, 0);
        //    transform.position += transform.forward * speed;
        //}


        float street = 20;
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * street * Time.deltaTime, 0);

        float Speed = 3f;
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * Speed * Time.deltaTime;
        transform.transform.position += s;

        //炮塔角度
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;

        //玩家控制操控
        PlayerCtrl();
        //遍历车轴
        foreach (AxleInfo axleInfo in axleInfos)
        {
            //转向
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //马力
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //制动
            if (true)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
            //转动轮子
            if(axleInfos[1] != null)
            {
                WheelsRotation(axleInfos[1].leftWheel);
                TrackMove();
            }
        }

        //炮塔炮管旋转
        TurretRotation();
        TurretRoll();
        //发送机音效
        MotorSound();

        ComputerCtrl();
        NoneCtrl();

    //    CalExplodePoint();

    }

    /// <summary>
    /// 炮塔旋转
    /// </summary>
    /// 
    //炮塔
    public Transform turret;
    //炮塔的旋转速度
    public float turretRotSpeed = 0.5f;
    //炮塔目标角度

    public float turretRotTarget = 0f;

    public void TurretRotation()
    {
        if (Camera.main == null)
            return;
        if (turret == null)
            return;

        //归一角度
        float angle = turret.eulerAngles.y - turretRotTarget;
        if (angle < 0)
            angle += 360;
        if (angle > turretRotSpeed && angle < 180)
            turret.Rotate(0f, -turretRotSpeed, 0f);
        else if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0f, turretRotSpeed, 0f);
    }

    //炮管
    public Transform gun;
    //炮管的旋转范围
    private float maxRoll = 10f;
    private float minRoll = -4f;

    //炮管目标角度
    private float turretRollTarget = 0;
    /// <summary>
    /// 炮管旋转
    /// </summary>
    public void TurretRoll()
    {
        if (Camera.main == null)
            return;
        if (turret == null)
            return;
        //获取角度
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        //世界坐标系角度计算 
        worldEuler.x = turretRollTarget;
        gun.eulerAngles = worldEuler;
        //本地坐标系角度限制
        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)
            euler.x -= 360;
        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x, localEuler.y, localEuler.z);
    }

    //轮轴
    public List<AxleInfo> axleInfos;
    //马力/最大马力
    private float motor = 0;
    public float MaxMotorTorque;
    //制动/最大制动
    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;
    //转向角/最大转向角
    private float steering = 0;
    public float maxSteeringAngle;
    /// <summary>
    /// 玩家控制
    /// </summary>
    public void PlayerCtrl()
    {

        //只有晚间操控的坦克才会生效
        if (ctrlType != CtrlType.player)
            return;
        //马力和转向角
        motor = MaxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        //制动 
        brakeTorque = 0;
        foreach (AxleInfo axlenInfo in axleInfos)
        {
            if (axlenInfo.leftWheel.rpm > 5 && motor < 0)//前进，按下“下”键
                brakeTorque = maxBrakeTorque;
            else if (axlenInfo.leftWheel.rpm < -5 && motor > 0)//后退时，按下“上”键
                brakeTorque = maxBrakeTorque;
            continue;
        }
        //炮塔炮管角度
        //turretRotTarget = Camera.main.transform.eulerAngles.y;
        //turretRollTarget = Camera.main.transform.eulerAngles.x;
        TargetSignPos();

        //发射炮弹
        if (Input.GetMouseButton(0))
            Shoot();
        //炮塔炮管角度
        //TargetSignPos();


    }

    //轮子
    private Transform wheels;
    /// <summary>
    /// 轮子旋转
    /// </summary>
    /// <param name="colider"></param>
    public void WheelsRotation(WheelCollider collider)
    {
        if (wheels == null)
            return;
        //获取旋转信息
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        //旋转每个轮子
        foreach(Transform wheel in wheels)
        {
            wheel.rotation = rotation;
        }
    }

    //履带
    private Transform tracks;
    /// <summary>
    /// 履带滚动
    /// </summary>
    public void TrackMove()
    {
        if (tracks == null)
            return;
        float offset = 0;
        if (wheels.GetChild(0) != null)
            offset = wheels.GetChild(0).localEulerAngles.x / 90f;

        foreach(Transform track in tracks)
        {
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
                return;
            Material mtl = mr.material;
            mtl.mainTextureOffset = new Vector2(0f, offset);
        }
    }


    //马达声音
    public AudioSource motorAudioSource;
    //马达音效
    public AudioClip motorClip;
    /// <summary>
    /// 马达音效
    /// </summary>
    void MotorSound()
    {
        if(motor != 0 && !motorAudioSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }
        else if(motor == 0)
        {
            motorAudioSource.Pause();
        }
    }

    //炮弹预设
    public GameObject bullet;
    //上次开炮时间
    public float lastShootTime = 0;
    //开炮的时间间隔
    private float shootInterval = 0.5f;
    /// <summary>
    /// 发射炮弹
    /// </summary>
    public void Shoot()
    {
        //发射间隔
        if (Time.time - lastShootTime < shootInterval)
            return;
        //子弹
        if (bullet == null)
            return;
        //发射
        Vector3 pos = gun.position + gun.forward * 5;
        GameObject bulletObj = (GameObject)Instantiate(bullet, pos, gun.rotation);
        Bullet bulletCmp = bulletObj.GetComponent<Bullet>();
        if (bulletCmp != null)
            bulletCmp.attackTank = this.gameObject;

        lastShootTime = Time.time;

        shootAudioSource.PlayOneShot(shootClip);

     //   BeAttacked(30);
    }


    /// <summary>
    /// 坦克受到攻击后的反应
    /// </summary>
    /// <param name="att"></param>
    public void BeAttacked(float att, GameObject attackTank)
    {
        //坦克已经被摧毁
        if (hp <= 0)
            return;
        //击中处理 
        if(hp>0)
        {
            hp -= att;
            //AI处理
            if(ai!=null)
            {
                ai.OnAttecked(attackTank);
            }
        }
        //被摧毁
        if(hp <=0)
        {
            GameObject DestoryObj = (GameObject)Instantiate(DestoryEffect);
            DestoryObj.transform.SetParent(transform, false);
            DestoryObj.transform.localPosition = Vector3.zero;
            ctrlType = CtrlType.none;
            //显示击杀提示
            if(attackTank != null)
            {
                Tank tankCmp = attackTank.GetComponent<Tank>();
                if (tankCmp != null && tankCmp.ctrlType == CtrlType.player)
                    tankCmp.StartDrawKill();
            }
            //战场结算
            Battle.instance.IsWin(attackTank);
        }
        

    }

    /// <summary>
    /// 计算目标角度
    /// </summary>
    public void TargetSignPos()
    {
        //碰撞信息和碰撞点
        Vector3 hitPoint = Vector3.zero;
        RaycastHit raycaseHit;
        //屏幕中心位置
        Vector3 centerVec = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(centerVec);
        //射线检测，获取hitPoint
        if(Physics.Raycast(ray, out raycaseHit, 400.0f))
        {
            hitPoint = raycaseHit.point;
        }
        else
        {
            hitPoint = ray.GetPoint(400);
        }
        //计算目标角度
        Vector3 dir = hitPoint - turret.position;
        Quaternion angle = Quaternion.LookRotation(dir);
        turretRotTarget = angle.eulerAngles.y;
        turretRollTarget = angle.eulerAngles.x;
        //调试用，稍后删除
        Transform targetCube = GameObject.Find("TargetCube").transform;
        targetCube.position = hitPoint;
    }


    /// <summary>
    /// 计算爆炸位置
    /// </summary>
    /// <returns></returns>
    public Vector3 CalExplodePoint()
    {
        //碰撞信息和碰撞点
        Vector3 hitPoint = Vector3.zero;
        RaycastHit hit;
        //沿着炮管方向的射线
        Vector3 pos = gun.position + gun.forward * 5;
        Ray ray = new Ray(pos, gun.forward);
        //射线检测
        if(Physics.Raycast(ray,out hit, 400.0f))
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = ray.GetPoint(400);
        }
        //调试用
        Transform explodeCube = GameObject.Find("ExplodeCube").transform;
        //调试用结束
        return hitPoint;
    }

    //准心
    public Texture2D centerSight;
    //坦克中心
    public Texture2D tankSight;
    /// <summary>
    /// 绘制准心
    /// </summary>
    public void DrawSight()
    {
        //计算实际射击位置
        Vector3 explodePoint = CalExplodePoint();
        //获取“坦克准心”的屏幕坐标
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(explodePoint);
        //绘制坦克准信
        Rect tankRect = new Rect(screenPoint.x - tankSight.width / 2, Screen.height - screenPoint.y - tankSight.height / 2, tankSight.width, tankSight.height);
        GUI.DrawTexture(tankRect, tankSight);
        //绘制中心准心
        Rect centerRect = new Rect(Screen.width / 2 - centerSight.width / 2, Screen.height / 2 - centerSight.height / 2, centerSight.width, centerSight.height);
        GUI.DrawTexture(centerRect, centerSight);
    }

    void OnGUI()
    {
        if (ctrlType != CtrlType.player)
            return;
        DrawSight();
        DrawHp();
    }

    //生命指示条素材
    public Texture2D hpBarBg;
    public Texture2D hpBar;
    /// <summary>
    /// 绘制生命条
    /// </summary>
    public void DrawHp()
    {
        //底框
        Rect bgRect = new Rect(30, Screen.height - hpBarBg.height - 15, hpBarBg.width, hpBarBg.height);
        GUI.DrawTexture(bgRect, hpBarBg);
        //指示条
        float width = hp * 102 / maxHp;
        Rect hpRect = new Rect(bgRect.x + 29, bgRect.y + 9, width, hpBar.height);
        GUI.DrawTexture(hpRect, hpBar);
        //文字
        string text = Mathf.Ceil(hp).ToString() + "/" + Mathf.Ceil(maxHp).ToString();
        Rect textRect = new Rect(bgRect.x + 80, bgRect.y - 10, 50, 50);
        GUI.Label(textRect, text);
    }

    //击杀提示图标
    public Texture2D killUI;
    //击杀图标开始显示的时间
    private float killUIStartTime = float.MinValue;
    /// <summary>
    ///  //显示击杀图标
    /// </summary>
    public void StartDrawKill()
    {
        killUIStartTime = Time.time;
    }

    private void DrawKIllUI()
    {
        if (Time.time - killUIStartTime < 1f)
        {
            Rect rect = new Rect(Screen.width / 2 - killUI.width / 2, 30, killUI.width, killUI.height);
            GUI.DrawTexture(rect, killUI);
        }
    }

    /// <summary>
    /// 电脑控制
    /// </summary>
    public void ComputerCtrl()
    {
        if (ctrlType != CtrlType.computer)
            return;

        //炮塔角度
        Vector3 rot = ai.GetTurretTarget();
        turretRotTarget = rot.y;
        turretRollTarget = rot.x;

       if(ai.IsShoot())
            Shoot();
        //移动
        steering = ai.GetSteering();
        motor = ai.GetMotor();
        brakeTorque = ai.GetBrakeTorque();
    }

    /// <summary>
    /// 无人控制
    /// </summary>
    public void NoneCtrl()
    {
        if (ctrlType != CtrlType.none)
            return;
        motor = 0;
        steering = 0;
        brakeTorque = maxBrakeTorque / 2;
    }
}
