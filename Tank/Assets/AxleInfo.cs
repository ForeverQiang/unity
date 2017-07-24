using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{

    //轴
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    //轮子是否转向
    public bool steering;
}
