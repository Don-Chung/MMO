using ARPGCommon;
using ARPGCommon.Model;
using ARPGCommon.Tools;
using ARPGServer.DB.Manager;
using ARPGServer.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    internal class RegisterHandler : HandlerBase
    {
        private UserManager manager;

        public RegisterHandler()
        {
            manager = new UserManager();
        }

        public override OperationCode OpCode 
        { 
             get { return OperationCode.Register; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            User user = ParameterTool.GetParameter<User>(request.Parameters, ARPGCommon.ParameterCode.User);
            User userDB = manager.GetUserByUsername(user.Username);
            if(userDB != null)
            {
                response.ReturnCode = (short)ReturnCode.Fail;
                response.DebugMessage = "用户名重复";
            }
            else
            {
                user.Password = MD5Tool.GetMD5(user.Password);
                manager.AddUser(user);
                response.ReturnCode = (short)ReturnCode.Success;
            }
        }
    }
}
