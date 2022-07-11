using ARPGCommon;
using ARPGCommon.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    internal class EnemyHandler : HandlerBase
    {
        public override OperationCode OpCode
        {
            get { return OperationCode.Enemy; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer,
            SendParameters sendParameters)
        {
            SubCode subcode = ParameterTool.GetSubCode(request.Parameters);
            switch (subcode)
            {
                case SubCode.CreateEnemy:
                    TransmitRequest(peer, request);
                    break;
                case SubCode.SyncPositionAndRotation:
                    TransmitRequest(peer, request);
                    break;
                case SubCode.SyncAnimation:
                    TransmitRequest(peer, request);
                    break;
            }
        }
        //这个方法用来转发请求
        void TransmitRequest(UserPeer peer, OperationRequest request)
        {
            foreach (UserPeer temp in peer.Team.clientPeers)
            {
                if (temp != peer)
                {
                    EventData data = new EventData();
                    data.Parameters = request.Parameters;
                    ParameterTool.AddOperationcodeSubcodeRoleID(data.Parameters, OpCode, peer.LoginRole.ID);
                    temp.SendEvent(data, new SendParameters());
                }
            }
        }
    }
}
