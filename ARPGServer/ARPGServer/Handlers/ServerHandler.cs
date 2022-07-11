using ARPGCommon;
using ARPGCommon.Model;
using ARPGServer.DB.Manager;
using LitJson;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    internal class ServerHandler : HandlerBase
    {
        private ServerPropertyManager manager;
        public ServerHandler()
        {
            manager = new ServerPropertyManager();
        }

        public override OperationCode OpCode
        {
            get { return OperationCode.GetServer; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            List<ServerProperty> list = manager.GetServerList();
            string json = JsonMapper.ToJson(list);
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.ServerList, json);

            response.OperationCode = request.OperationCode;
        }
    }
}
