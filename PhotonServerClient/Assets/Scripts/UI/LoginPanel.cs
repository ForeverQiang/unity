using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{


    public GameObject registerButton;
    public InputField usernameIF;
    public InputField passwordIF;
    public Text hintMessage;

    private LoginResquest loginRequest;

    private void Start()
    {
        loginRequest = GetComponent<LoginResquest>();
    }

    public void OnLoginButton()
    {
        hintMessage.text = "";
        loginRequest.Username = usernameIF.text;
        loginRequest.Password = passwordIF.text;
        loginRequest.DefaultRequest();
    }

    public void OnRegisterButton()
    {
        gameObject.SetActive(false);
        registerButton.SetActive(true);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SceneManager.LoadScene("Game");
        } 
        else
        {
            hintMessage.text = "用户名或密码错误";
        }
    }
}
