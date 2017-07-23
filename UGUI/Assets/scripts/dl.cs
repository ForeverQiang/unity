using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Xml;

public class dl : MonoBehaviour {

    private string _xmlpath;


    void Start()
    {
        _xmlpath = Application.dataPath + "/uer.xml";
       
        if(!File.Exists(_xmlpath))
        {
            //新建xml实力
            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点，最上层节点
            XmlElement root = xmlDoc.CreateElement("Root");
            xmlDoc.AppendChild(root);
            //创建用户子节点
            XmlElement user = xmlDoc.CreateElement("User");
            user.SetAttribute("user_name", "Admin");
            user.SetAttribute("user_pass", "I5188");
            user.SetAttribute("user_tel", "123456778");
            user.SetAttribute("user_qq", "982227460");
            root.AppendChild(user);
            xmlDoc.Save(_xmlpath);
            Debug.Log("xml create success");
        }
    }

    void Update()
    {
        
    }



    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.quit();
#endif

    }

    public Image image_zc;
    
    public void open_zc()
    {
            image_zc.gameObject.SetActive(true);

    }
    public void close_zc()
    {
            image_zc.gameObject.SetActive(false);
    }

    public Image image_ty_xxk;
    public Text text_info;
    public Text text_ts;
    public void ty_xxk( string str)
    {
        image_ty_xxk.gameObject.SetActive(true);
        text_info.text = str;
        text_ts.text = str;
    }
    public void ty_xxk_clost()
    {
        image_ty_xxk.gameObject.SetActive(false);
    }
    /*
        public void test()
        {
            ty_xxk("用户名或密码不能为空！");
        }
        */

    public InputField InputField_name_zc;
    public InputField Inputfield_pass_zc;
    public InputField InputField_pass1_zc;
    public InputField InputField_tel_zc;
    public InputField InputField_qq_zc;

    
    //注册函数
    public void zc()
    {
        if(InputField_name_zc.text == ""|| Inputfield_pass_zc.text == "" || InputField_pass1_zc.text == "" || InputField_qq_zc.text == "" || InputField_tel_zc.text == "")
        {
            ty_xxk("信息不完整，请填写完整");
            return;
        }
        if(Inputfield_pass_zc.text != InputField_pass1_zc.text)
        {
            ty_xxk("密码不一致");
            return;
        }
        //判断用户账号是否被占用！
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(_xmlpath);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        foreach(XmlElement xe in nodeList)
        {
            if(xe.GetAttribute("user_name") == InputField_name_zc.text)
            {
                ty_xxk("该用户已经存在");
                return;
            }
        }
        //    ty_xxk("该用户没有被占用，可以使用！");
        //获取根节
        XmlNode root = xmlDoc.SelectSingleNode("Root");
        //创建user新节点
        XmlElement user = xmlDoc.CreateElement("User");
        //写入注册信息
        user.SetAttribute("user_name", InputField_name_zc.text);
        user.SetAttribute("user_pass", Inputfield_pass_zc.text);
        user.SetAttribute("user_tel", InputField_tel_zc.text);
        user.SetAttribute("user_qq", InputField_qq_zc.text);
        //将节点加入根节点
        root.AppendChild(user);
        //保存文件
        xmlDoc.Save(_xmlpath);
        //清空注册信息
        InputField_name_zc.text = "";
        Inputfield_pass_zc.text = "";
        InputField_tel_zc.text = "";
        InputField_qq_zc.text = "";
        InputField_pass1_zc.text = "";
    }

    public InputField InputField_name;
    public InputField InputField_pass;
    public void login()
    {
        if(InputField_name.text =="" || InputField_pass.text=="")
        {
            ty_xxk("密码或者账号为空，请重新登陆！");
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(_xmlpath);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        foreach(XmlElement xe in nodeList)
        {
            if(xe.GetAttribute("user_name") == InputField_name.text)
            {
                var mima = xe.GetAttribute("user_pass");
                if(InputField_pass.text == mima)
                {
                    ty_xxk("登陆成功");
                    Application.OpenURL("http:www.baidu.com");
                    return;
                }
                else
                {
                    ty_xxk("登陆失败，密码错误");
                    return;
                }
            }
        }
        ty_xxk("此用户还没有注册，请注册");
    }  
}
