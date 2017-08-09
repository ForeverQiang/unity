using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : PanelBase {

    private Text text;
    private Button btn;
    string str = "";

    #region 生命周期 

    //初始化 
    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "TipPanel";
        layer = PanelLayer.Tips;

        //参数args[i]代表提示的内容
        if(args.Length == 1)
        {
            str = (string)args[0];
        }
    }

    //显示之前
    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;
        //文字
        text = skinTrans.Find("Text").GetComponent<Text>();
        text.text = str;
        //关闭按钮
        btn = skinTrans.Find("Btn").GetComponent<Button>();
        btn.onClick.AddListener(onBtnClick);
    }
    #endregion

    //按下“知道了"按钮的事件
    public void OnBtnClick()
    {
        Close();
    }

}
