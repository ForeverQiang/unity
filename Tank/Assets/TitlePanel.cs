using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : PanelBase {

    private Button startBtn;
    private Button infoBtn;

    #region 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TitlePanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform.FindChild("StartBtn").GetComponent<Button>();
        infoBtn = skinTrans.FindChild("InfoBtn").GetComponent<Button>();

        startBtn.onClick.AddListener(OnStartClick);
        infoBtn.onClick.AddListener(OnInfoClick);
    }
    #endregion

    public void OnStartClick()
    {
        //开始游戏
        // Battle.instance.StartTwoCampBattle(2, 2);
        //关闭
        // Close();
        //设置
        PanelMgr.instance.OpenPanel<OptionPanel>("");
    }

    public void OnInfoClick()
    {
        PanelMgr.instance.OpenPanel<InfoPanel>("");
    }
}
