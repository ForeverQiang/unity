/**************************************************************************************************************
 * 作    者： 吴军强
 * CLR 版本： 4.0.30319.42000
 * 创建时间： 10/20/2017 4:59:01 PM
 * 当前版本： 1.0.0.1
 * 编写系统： ASUS-PC
 * 区    域： ASUS-PC
 * 描述说明：
 * 修改历史：
 * ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Common;
using MyGameServer.Handler;
using MyGameServer.Threads;

namespace MyGameServer
{
    // 说有的server端，主类都要集成子applicationbase
    public class MyGameServer : ApplicationBase
    {
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static MyGameServer Instance
        {
            get;
            private set;
        }
        public Dictionary<OperationCode, BaseHandler> HandlerDic = new Dictionary<OperationCode, BaseHandler>();
        public List<ClientPeer> peerList = new List<ClientPeer>();//通过这个集合可以访问所有客户端的集合
        private SyncPositionThread syncPositionThread = new SyncPositionThread();
        //当一个客户端请求连接的
        //使用peerbase，表示和一个客户端的连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("有个新的客户端连接成功！");
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }


        //初始化
        protected override void Setup()
        {
            Instance = this;
            //日志的初始化
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "Log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);//让log4net读取配置文件
            }

            log.Info("Setup Complete");

            InitHandler();
            syncPositionThread.Run();
        }

        public void InitHandler()
        {
            LoginHandler loginHandler = new LoginHandler();
            HandlerDic.Add(loginHandler.OpCode, loginHandler);
            DefaultHandler defaultHandler = new DefaultHandler();
            HandlerDic.Add(defaultHandler.OpCode, defaultHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            HandlerDic.Add(registerHandler.OpCode, registerHandler);
            SyncPositionHandler syncPositionHandler = new SyncPositionHandler();
            HandlerDic.Add(syncPositionHandler.OpCode, syncPositionHandler);
            SyncPlayerHandler syncPlayerHandler = new SyncPlayerHandler();
            HandlerDic.Add(syncPlayerHandler.OpCode, syncPlayerHandler);
            log.Info("初始化");
        }
        //server端关闭的时候
        protected override void TearDown()
        {
            log.Info("服务器关闭了");
            syncPositionThread.Stop();
        }
    }
}
