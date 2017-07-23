using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class image_I_10 : MonoBehaviour
{

    public Image image_sp;
    public Text text_sp;
    public Text text_btn;

    public void open_close()
    {
        if (text_btn.text == "打开图片")
        {
            image_sp.gameObject.SetActive(true);
            text_btn.text = "关闭图片";
            text_sp.text = "sdafffffffffasdf";
        }
        else
        {
            image_sp.gameObject.SetActive(false);
            text_btn.text = "打开图片";
            text_sp.text = "东方马德里？";
        }
    }


}
