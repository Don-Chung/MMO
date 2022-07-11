using ARPGCommon;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    public abstract class HandlerBase
    {
        public HandlerBase()
        {
            ArpgApplication._Instance.handlers.Add((byte)OpCode, this);
        }
        public abstract void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters);
        public abstract OperationCode OpCode { get; }
    }
}
