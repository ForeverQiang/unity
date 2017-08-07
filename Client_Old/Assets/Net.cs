using System;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Net : MonoBehaviour {

    ////与服务器的套接字
    //Socket socket;
    ////服务器的IP和端口
    //public InputField hostInput;
    //public InputField portInput;
    ////文本框
    //public Text recvText;
    //public Text clientText;
    ////接收缓冲区
    //const int BUFFER_SIZE = 1024;
    //byte[] readbuff = new byte[BUFFER_SIZE];

    //public void Connection()
    //{
    //    //Socket
    //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //    //connect
    //    string host = hostInput.text;
    //    int port = int.Parse(portInput.text);
    //    socket.Connect(host, port);
    //    clientText.text = "客户端地址" + socket.LocalEndPoint.ToString();
    //    Debug.Log(socket.LocalEndPoint.ToString());
    //    //send
    //    string str = "hello Unity!";
    //    byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
    //    socket.Send(bytes);
    //    //Recv
    //    int count = socket.Receive(readbuff);
    //    str = System.Text.Encoding.UTF8.GetString(readbuff, 0, count);
    //    recvText.text = str;
    //    //Close
    //    socket.Close();
    //}

    //服务器IP和端口
    public InputField hostInput;
    public InputField portInput;
    //显示客户端收到的消息
    public Text recvText;
    public string recvStr;
    //显示客户端IP和端口
    public Text clientText;
    //聊天输入框
    public InputField textInput;
    //Scoket和接收缓冲区
    Socket socket;
    const int BUFFER_SIZE = 1024;
    public byte[] readbuff = new byte[BUFFER_SIZE];

    //因为只有主线程能够修改ＵＩ组件的属性
    //因此在UPdate里更换文本
    private void Update()
    {
        recvText.text = recvStr;
    }

    //连接
    public void Connetion()
    {
        //清理Text
        recvText.text = "";
        //Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Connect
        string host = hostInput.text;
        int port = int.Parse(portInput.text);
        socket.Connect(host, port);
        clientText.text = " 客户端地址 " + socket.LocalEndPoint.ToString();
        //Recv
        socket.BeginReceive(readbuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    //回调
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            //count是接收数据的大小
            int count = socket.EndReceive(ar);
            //数据处理
            string str = System.Text.Encoding.UTF8.GetString(readbuff, 0, count);
            if (recvStr.Length > 300)
                recvStr = "";
            recvStr += str + "\n";
            //继续接收
            socket.BeginReceive(readbuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            recvText.text += "链接已断开";
            socket.Close();
        }
    }

    //发送数据
    public void Send()
    {
        string str = textInput.text;
        byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
        byte[] length = BitConverter.GetBytes(bytes.Length);
        byte[] sendbuff = length.Concat(bytes).ToArray();
        socket.Send(sendbuff);
        //try
        //{
        //    socket.Send(bytes);
        //}
        //catch { }
    }
}
