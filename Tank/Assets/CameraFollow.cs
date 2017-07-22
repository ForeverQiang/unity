using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float distance = 15;
    public float rot = 0;
    private float roll = 30f * Mathf.PI * 2 / 360;

    private GameObject target;

    private void Start()
    {
        target = GameObject.Find("tank");
        //SetTarget(GameObject.Find("Tank"));

    }

    private void LateUpdate()
    {
        if (target == null)
            return;
        if (Camera.main == null)
            return;

        Rotate();
        Roll();
        Zoom();

        Vector3 targetPos = target.transform.position;

        Vector3 CameraPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        CameraPos.x = targetPos.x + d * Mathf.Sin(roll);
        CameraPos.z = targetPos.z + d * Mathf.Cos(roll);
        CameraPos.y = targetPos.y + height;

        Camera.main.transform.position = CameraPos;

        Camera.main.transform.LookAt(target.transform);
    }


    /// <summary>
    /// 中心点
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        if (target.transform.Find("cameraPoint") != null)
            this.target = target.transform.Find("cameraPoint").gameObject;
        else
            this.target = target;
    }

    public float rotSpeed = 0.2f;


    /// <summary>
    /// 横向旋转相机
    /// </summary>
    void Rotate()
    {
        float w = Input.GetAxis("Mouse X") * rotSpeed;
        rot -= w;
    }

    /// <summary>
    /// 纵向旋转相机
    /// </summary>
    private float maxRoll = 70f * Mathf.PI * 2 / 360;
    private float minRoll = -10f * Mathf.PI * 2 / 360;

    private float rollSpeed = 0.2f;
    void Roll()
    {
        float w = Input.GetAxis("Mouse Y") * rollSpeed * 0.5f;

        roll -= w;
        if (roll > maxRoll)
            roll = maxRoll;
        if (roll < minRoll)
            roll = minRoll;
    }


    /// <summary>
    ///调整距离
    /// </summary>
    public float maxDistance = 22f;
    public float minDistance = 5;

    public float zoomSpeed = 0.2f;

    void Zoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            if (distance > minDistance)
                distance -= zoomSpeed;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance < maxDistance)
                distance += zoomSpeed;
        }
    }
}
