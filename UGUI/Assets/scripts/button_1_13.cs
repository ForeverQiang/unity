using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class button_1_13 : MonoBehaviour {

    public Image image_sp;
    public Sprite[] image_sps;//图片数组
    public string select_name;
    private int i;

    public void sp()
    {
        select_name = EventSystem.current.currentSelectedGameObject.name;
        if(select_name == "Button_left")
        {
           if(i<= 0)
            {
                i = image_sps.Length - 1;
            }
            image_sp.overrideSprite = image_sps[--i];//这里换图片
        }
        else
        {
            if(i>=image_sps.Length - 1 )
            {
                i = 0;
            }
            image_sp.overrideSprite = image_sps[++i];//这里换图片
        }
    }
}
