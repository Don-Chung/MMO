using ARPGCommon;
using ARPGCommon.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Tools
{
    public class RequestTool
    {
        public static void TransmitRequest(UserPeer peer, OperationRequest request, OperationCode opCode)
        {
            foreach (UserPeer temp in peer.Team.clientPeers)
            {
                if (temp != peer)
                {
                    EventData data = new EventData();
                    data.Parameters = request.Parameters;
                    ParameterTool.AddOperationcodeSubcodeRoleID(data.Parameters, opCode, peer.LoginRole.ID);
                    temp.SendEvent(data, new SendParameters());
                }
            }
        }
    }
}
