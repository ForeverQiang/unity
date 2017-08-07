﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegPanel : MonoBehaviour {

    private InputField idInput;
    private InputField pwInput;
    private Button regBtn;
    private Button closeBtn;

    #region 生命周期
    //初始化
    private override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "RegPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        idInput = skinTrans.FindChild("IDInput").GetComponent<InputField>();
        pwInput = skinTrans.FindChild("PWInput").GetComponent<InputField>();
        regBtn = skinTrans.FindChild("RegBtn").GetComponent<Button>();
        closeBtn = skinTrans.FindChild("CloseBtn").GetComponent<Button>();

        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(onCloseClick);
    }


    public void OnCloseClick()
    {
        PanelMgr.instance.OpenPanel<LoginPanel>("");
        Close();
    }

    public void OnRegClick()
    {
        //用户名、密码为空
        if(idInput.text == "" || pwInput.text == "")
        {
            Debug.Log("用户名密码不能为空");
            return;
        }

        if(NetMgr.SrvConn.status != Connection.Status.Connected )
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.SrvConn.proto = new ProtocolBytes();
            NetMgr.SrvConn.Connect(host, port);
        }

        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Register");
        protocol.AddString(idInput.text);
        protocol.AddString(pwInput.text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.SrvConn.Send(protocol, onRegBack);
    }

    public void OnregBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string ProtoName = proto.GetString(start, ref start);

        int ret = proto.GetInt(start, ref start);

        if(ret == 0)
        {
            Debug.Log("注册成功");
            PanelMgr.instance.OpenPanel<LoginPanel>("");
            closeBtn();
        }
        else
        {
            Debug.Log("注册失败");
        }
    }
#endregion
}
