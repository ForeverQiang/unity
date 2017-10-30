using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.Tools;

public class Player : MonoBehaviour
{

    public bool isLoadPlayer = true;
    public string username;

    public GameObject PlayerPrefab;
    public GameObject player;

    private SyncPositionRequest syncPosRequest;
    private SyncPlayerRequest syncPlayerRequest;
    private Vector3 lastPosition = Vector3.zero;
    public float moveoffset = .1f;

    private Dictionary<string, GameObject> playerDict = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        //if (isLoadPlayer)
        //{
        syncPosRequest = GetComponent<SyncPositionRequest>();
        syncPlayerRequest = GetComponent<SyncPlayerRequest>();
        syncPlayerRequest.DefaultRequest();
        player.GetComponent<Renderer>().material.color = Color.green;
        InvokeRepeating("SyncPosition", 3f, 0.1f);
        // }
    }

    void SyncPosition()
    {
        if (Vector3.Distance(transform.position, lastPosition) > moveoffset)
        {
            lastPosition = player.transform.position;
            syncPosRequest.pos = player.transform.position;
            syncPosRequest.DefaultRequest();

        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (isLoadPlayer)
        //{
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        player.transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * 4);
        //}
    }
    public void OnSyncPlayerReponse(List<string> usernameList)
    {
        //创建其他客户端的player角色
        foreach (var username in usernameList)
        {
            OnNewPlaerEvent(username);
        }
    }

    public void OnNewPlaerEvent(string username)
    {
        GameObject go = GameObject.Instantiate(PlayerPrefab);
        go.GetComponent<Renderer>().material.color = Color.red;

        //DestroyImmediate (GetComponent<SyncPlayerRequest>());
        //DestroyImmediate(GetComponent<SyncPositionRequest>());
        //DestroyImmediate(GetComponent<NewPlayerEvent>());
        //DestroyImmediate(GetComponent<SyncPositionEvent>());
        // go.GetComponent<Player>().isLoadPlayer = false; //其他客户端的角色
        // go.GetComponent<Player>().username = username;
        playerDict.Add(username, go);
    }

    public void OnSyncPositonEvent(List<PlayerData> playerDataList)
    {
        foreach (PlayerData pd in playerDataList)
        {

            GameObject go = DictTool.GetValue<string, GameObject>(playerDict, pd.Username);
            if (go != null)
            {
                go.transform.position = new Vector3() { x = pd.Pos.x, y = pd.Pos.y, z = pd.Pos.z };
            }
        }
    }

}
