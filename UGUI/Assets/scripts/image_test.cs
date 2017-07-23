using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class image_test : MonoBehaviour {

    public Image image1;
    public float i = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        image1.type = Image.Type.Filled;
        image1.fillMethod = Image.FillMethod.Radial360;
        image1.fillAmount = i;
        if(i >= 1.0f)
        {
            i = 0.0f;
        }
        i = i + 0.001f;
	}
}
