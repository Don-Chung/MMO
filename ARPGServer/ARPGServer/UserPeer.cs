using ARPGCommon.Model;
using ARPGServer.Handlers;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer
{
    public class UserPeer : ClientPeer
    {
        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
        public User LoginUser { get; set; } //存储当前登录的账号
        public Role LoginRole { get; set; }

        public Team Team { get; set; }
        public UserPeer(InitRequest initRequest) : base(initRequest)
        {
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            if (ArpgApplication._Instance.clientPeerListForTeam.Contains(this))
            {
                ArpgApplication._Instance.clientPeerListForTeam.Remove(this);
            }
            log.Debug("A client is disconnect.");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            HandlerBase handler;
            ArpgApplication._Instance.handlers.TryGetValue(operationRequest.OperationCode, out handler);
            OperationResponse response = new OperationResponse();
            response.OperationCode = operationRequest.OperationCode;
            response.Parameters = new Dictionary<byte, object>();
            if(handler != null)
            {
                handler.OnHandlerMessage(operationRequest, response, this, sendParameters);
                SendOperationResponse(response, sendParameters);
            }
            else
            {
                log.Debug("Can't find handler from operation code:" + operationRequest.OperationCode);
            }
        }
    }
}
