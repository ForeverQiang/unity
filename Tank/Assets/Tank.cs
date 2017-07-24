using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{

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
        turretRotTarget = Camera.main.transform.eulerAngles.y;
        turretRollTarget = Camera.main.transform.eulerAngles.x;
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



}
