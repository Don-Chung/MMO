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
    internal class SkillDBHandler : HandlerBase
    {
        private SkillDBManager skillDBManager;
        public SkillDBHandler()
        {
            skillDBManager = new SkillDBManager();
        }
        public override OperationCode OpCode 
        { 
            get { return OperationCode.SkillDB; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            SubCode subCode = ParameterTool.GetSubCode(request.Parameters);
            ParameterTool.AddSubCode(response.Parameters, subCode);
            switch (subCode)
            {
                case SubCode.Add:
                    SkillDB skillDB = ParameterTool.GetParameter<SkillDB>(request.Parameters, ParameterCode.SkillDB);
                    skillDB.Role = peer.LoginRole;
                    skillDBManager.Add(skillDB);
                    skillDB.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDB, skillDB);
                    break;
                case SubCode.Get:
                    List<SkillDB> list = skillDBManager.Get(peer.LoginRole);
                    foreach(var temp in list)
                    {
                        temp.Role = null;
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDBList, list);
                    break;
                case SubCode.Update:
                    SkillDB skillDB2 = ParameterTool.GetParameter<SkillDB>(request.Parameters, ParameterCode.SkillDB);
                    skillDB2.Role = peer.LoginRole;
                    skillDBManager.Update(skillDB2);
                    break;
                case SubCode.Upgrade:
                    SkillDB skillDB3 = ParameterTool.GetParameter<SkillDB>(request.Parameters, ParameterCode.SkillDB);
                    Role role = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    role.User = peer.LoginUser;
                    skillDB3.Role = role;
                    skillDBManager.Upgrade(skillDB3, role);
                    skillDB3.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDB, skillDB3);
                    break;
            }

        }
    }
}
