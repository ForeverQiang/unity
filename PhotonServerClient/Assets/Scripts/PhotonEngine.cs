using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using Common;
using Common.Tools;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{

    public static PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }

    public static PhotonEngine Instance;
    private static PhotonPeer peer;
    private Dictionary<OperationCode, Request> RequestDic = new Dictionary<OperationCode, Request>();
    private Dictionary<EventCode, BaseEvent> EventDict = new Dictionary<EventCode, BaseEvent>();
    public static string username;

    public void DebugReturn(DebugLevel level, string message)
    {
      //  throw new NotImplementedException();
    }

    public void OnEvent(EventData eventData)
    {
        EventCode code = (EventCode)eventData.Code;
        BaseEvent e =  DictTool.GetValue<EventCode, BaseEvent>(EventDict, code);
        e.OnEvent(eventData);


       //switch(eventData.Code)
       // {
       //     case 1:
       //         Debug.Log("服务器发送来了事件");
       //         Dictionary<byte, object> data = eventData.Parameters;
       //         object intValue;
       //         object stringValue;
       //         data.TryGetValue(1, out intValue);
       //         data.TryGetValue(2, out stringValue);
       //         Debug.Log("Event:" + intValue.ToString() + "   " + stringValue.ToString());
       //         break;
       //     default:
       //         break;
       // }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = null;
        bool temp =  RequestDic.TryGetValue(opCode, out request);

        if(temp )
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("没有找到对应的响应处理对象");
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
        throw new NotImplementedException();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }


    // Use this for initialization
    void Start()
    {
        //通过Listender接收服务器的相应
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "MyGame1");

    }

    // Update is called once per frame
    void Update()
    {
        //if(peer.PeerState == PeerStateValue.Connected)
        //{
        peer.Service();
        //}
    }

    void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    public void AddRequest(Request request)
    {
        RequestDic.Add(request.OpCode, request);
    }

    public void RemoveRequest(Request request)
    {
        RequestDic.Remove(request.OpCode);
    }

    public void AddEvent(BaseEvent e)
    {
        EventDict.Add(e.EventCode, e);
    }

    public void RemoveEvent(BaseEvent e)
    {
        EventDict.Remove(e.EventCode);
    }
}
