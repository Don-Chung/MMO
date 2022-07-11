using ARPGCommon;
using ARPGCommon.Model;
using ARPGCommon.Tools;
using ARPGServer.DB.Manager;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer.Handlers
{
    public class RoleHandler : HandlerBase
    {
        private RoleManager roleManager = null;

        public RoleHandler()
        {
            roleManager = new RoleManager();
        }

        public override OperationCode OpCode 
        {
            get { return OperationCode.Role; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            //得到子操作代码
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.SubCode, subCode);
            response.OperationCode = request.OperationCode;

            switch (subCode)
            {
                case SubCode.AddRole:
                    Role role = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    role.User = peer.LoginUser;
                    roleManager.AddRole(role);
                    role.User = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.Role, role);
                    break;
                case SubCode.GetRole:
                    List<Role> roleList = roleManager.GetRoleListByUser(peer.LoginUser);
                    foreach(var role1 in roleList)
                    {
                        role1.User = null;
                    }
                    ParameterTool.AddParameter(parameters, ParameterCode.RoleList, roleList);
                    break;
                case SubCode.SelectRole:
                    peer.LoginRole = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    break;
                case SubCode.UpdateRole:
                    Role role2 = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    role2.User = peer.LoginUser;
                    roleManager.UpdateRole(role2);
                    role2.User = null;
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
            }
        }
    }
}
