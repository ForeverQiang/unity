using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour {

    public GameObject loginPanel;
    public InputField usernameIF;
    public InputField passwordIF;
    public Text hintMessage;

    private RegisterRequest registerRequest;

    private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();
    }

    public void OnRegisterButton()
    {
        hintMessage.text = ""; 
        registerRequest.username = usernameIF.text;
        registerRequest.password = passwordIF.text;
        registerRequest.DefaultRequest();

          
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        loginPanel.SetActive(true);

    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Success)
        {
            hintMessage.text = "注册成功,请返回登陆";
        }
        else if(returnCode == ReturnCode.Failed)
        {
            hintMessage.text = "用户名重复，请更改用户名";
        }
    }
}
