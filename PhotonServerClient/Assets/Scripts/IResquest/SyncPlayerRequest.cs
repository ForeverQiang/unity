﻿using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common.Tools;
using Common;
using System.Xml.Serialization;
using System.IO;

public class SyncPlayerRequest : Request{

    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void DefaultRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string usernameListString = (string)DictTool.GetValue<byte, object>(operationResponse.Parameters, (byte)ParameterCode.UsernameList);

        using (StringReader reader = new StringReader(usernameListString))
        {
            XmlSerializer serizlizer = new XmlSerializer(typeof(List<string>));
            List<string > usernameList = (List<string>) serizlizer.Deserialize(reader);
            Debug.Log("XML: "+serizlizer);

            player.OnSyncPlayerReponse(usernameList);
        }
    }


}
