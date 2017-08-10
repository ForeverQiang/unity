using Serv.Logic;
using System;
using System.Collections.Generic;


public class PlayerTempData
{
	public PlayerTempData()
	{
        status = Status.None;
	}

    //״̬
    public enum Status
    {
        None,
        Room,
        Fight,
    }

    public Status status;
    //room״̬
    public Room room;
    public int team = 1;
    public bool isOwner = false;

    //战场相关
    public long lsatUpdateTime;
    public float posX;
    public float PosY;
    public float posZ;
    public long lastShootTime;
    public float hp = 100;
}