using ARPGCommon;
using ARPGCommon.Model;
using ARPGServer.DB.Manager;
using ARPGServer.Tools;
using LitJson;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    public class LoginHandler : HandlerBase
    {
        private UserManager manager;
        public LoginHandler()
        {
            manager = new UserManager();
        }

        public override OperationCode OpCode 
        { 
            get { return OperationCode.Login; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            Dictionary<byte, object> parameters = request.Parameters;
            object jsonObeject = null;
            parameters.TryGetValue((byte)ParameterCode.User, out jsonObeject);
            User user = JsonMapper.ToObject<User>(jsonObeject.ToString());
            User userDB = manager.GetUserByUsername(user.Username);
            if(userDB != null && userDB.Password == MD5Tool.GetMD5(user.Password))
            {
                //用户名和密码正确
                response.ReturnCode = (short)ReturnCode.Success;
                peer.LoginUser = userDB;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Fail;
                response.DebugMessage = "用户名或密码错误！";
            }
        }
    }
}
