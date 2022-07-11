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
    public class TaskDBHandler : HandlerBase
    {
        private TaskDBManager taskDBManager;
        public TaskDBHandler()
        {
            taskDBManager = new TaskDBManager();
        }
        public override OperationCode OpCode 
        { 
            get { return OperationCode.TaskDB; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            response.Parameters.Add((byte)ParameterCode.SubCode, subCode);
            switch (subCode)
            {
                case SubCode.AddTaskDB:
                    TaskDB taskDB = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDB.Role = peer.LoginRole;
                    taskDBManager.AddTaskDB(taskDB);
                    taskDB.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.TaskDB, taskDB);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.GetTaskDB:
                    List<TaskDB> list = taskDBManager.GetTaskDBList(peer.LoginRole);
                    foreach(var TaskDb in list)
                    {
                        TaskDb.Role = null;
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.TaskDBList, list);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.UpdateTaskDB:
                    TaskDB taskDB2 = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDB2.Role = peer.LoginRole;
                    taskDBManager.UpdateTaskDB(taskDB2);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
            }
        }
    }
}
