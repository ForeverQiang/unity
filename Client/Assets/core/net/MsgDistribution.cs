using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//消息分发
public class MsgDistribution {

    //每帧处理消息的数量
    public  int num = 15;
    //消息列表
    public List<ProtocolBase> msgList = new List<ProtocolBase>();
    //委托类型
    public delegate void Delegate(ProtocolBase proto);
    //事件监听
    private Dictionary<string, Delegate> evenDict = new Dictionary<string, Delegate>();
    private Dictionary<string, Delegate> onceDict = new Dictionary<string, Delegate>();
}
