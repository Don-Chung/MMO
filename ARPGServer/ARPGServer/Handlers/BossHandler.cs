using ARPGCommon;
using ARPGCommon.Tools;
using ARPGServer.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    internal class BossHandler : HandlerBase
    {
        public override OperationCode OpCode
        {
            get { return OperationCode.Boss; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            SubCode subcode = ParameterTool.GetSubCode(request.Parameters);
            switch (subcode)
            {
                case SubCode.SyncBossAnimation:
                    RequestTool.TransmitRequest(peer, request, OpCode);
                    break;
            }
        }
    }
}
