using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class input_1_17 : MonoBehaviour {

    public Text text_jg;
    public InputField input1;
    public InputField input2;
    public InputField input_tc;

    public GameObject panel_tc;

    public void js()
    {
        text_jg.text = (int.Parse(input1.text) - int.Parse(input2.text)).ToString();
    }

    public void tc()
    {
        panel_tc.gameObject.SetActive(true);
    }

    public void yes()
    {
        text_jg.text = input_tc.text;
        panel_tc.gameObject.SetActive(false);
    }
    public void no()
    {
        panel_tc.gameObject.SetActive(false);
    }
}
