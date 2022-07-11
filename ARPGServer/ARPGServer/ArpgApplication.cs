using ARPGCommon;
using ARPGServer.Handlers;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer
{
    public class ArpgApplication : ApplicationBase
    {
        private static ArpgApplication _instance;

        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
        public Dictionary<byte, HandlerBase> handlers = new Dictionary<byte, HandlerBase>();

        public static ArpgApplication _Instance
        {
            get { return _instance; }
        }

        public List<UserPeer> clientPeerListForTeam = new List<UserPeer>();

        public ArpgApplication()
        {
            _instance = this;
            RegisterHandlers();
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new UserPeer(initRequest);
        }

        protected override void Setup()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "RPG" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));

            log.Debug("Apliaction setup complete..");
        }

        void RegisterHandlers()
        {
            //handlers.Add((byte)OperationCode.Login, new LoginHandler());//把LoginHandler交给ArpgApplication管理
            //handlers.Add((byte)OperationCode.GetServer, new ServerHandler());
            //handlers.Add((byte)OperationCode.Register, new RegisterHandler());

            //反射
            Type[] types = Assembly.GetAssembly(typeof(HandlerBase)).GetTypes();
            foreach(var type in types)
            {
                if (type.FullName.EndsWith("Handler"))
                {
                    Activator.CreateInstance(type);
                }
            }
        }

        protected override void TearDown()
        {
            log.Debug("Apliaction teardown..");
        }
    }
}
