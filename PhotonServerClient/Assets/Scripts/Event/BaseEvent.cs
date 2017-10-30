using Common;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour {

    public EventCode EventCode;

    public abstract void OnEvent(EventData eventData);

    public virtual void Start()
    {
        PhotonEngine.Instance.AddEvent(this);
    }

    public virtual void OnDestory()
    {
        PhotonEngine.Instance.RemoveEvent(this);
    }
}
