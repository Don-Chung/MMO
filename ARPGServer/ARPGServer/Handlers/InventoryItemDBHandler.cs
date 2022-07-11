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
    public class InventoryItemDBHandler : HandlerBase
    {
        private InventoryItemDBManager inventoryItemDBManager = new InventoryItemDBManager();

        public override OperationCode OpCode 
        { 
            get { return OperationCode.InventoryItemDB; }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, UserPeer peer, SendParameters sendParameters)
        {
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            ParameterTool.AddParameter(response.Parameters, ParameterCode.SubCode, subCode, false);
            switch (subCode)
            {
                case SubCode.GetInventoryItemDB:
                    List<InventoryItemDB> list =  inventoryItemDBManager.GetInventoryItemDB(peer.LoginRole);
                    foreach(var temp in list)
                    {
                        temp.Role = null;
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.InventoryItemDBList, list);
                    break;
                case SubCode.AddInventoryItemDB:
                    InventoryItemDB itemDB = ParameterTool.GetParameter<InventoryItemDB>(request.Parameters, 
                        ParameterCode.InventoryItemDB);
                    itemDB.Role = peer.LoginRole;
                    inventoryItemDBManager.AddInventoryItemDB(itemDB);
                    itemDB.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.InventoryItemDB, itemDB);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.UpdateInventoryItemDB:
                    InventoryItemDB itemDB2 = ParameterTool.GetParameter<InventoryItemDB>(request.Parameters,
                        ParameterCode.InventoryItemDB);
                    itemDB2.Role = peer.LoginRole;
                    inventoryItemDBManager.UpdateInventoryItemDB(itemDB2);
                    break;
                case SubCode.UpdateInventoryItemDBList:
                    List<InventoryItemDB> list2 = ParameterTool.GetParameter<List<InventoryItemDB>>(request.Parameters,
                        ParameterCode.InventoryItemDBList);
                    foreach(var itemDB3 in list2)
                    { 
                        itemDB3.Role = peer.LoginRole;
                    }
                    inventoryItemDBManager.UpdateInventoryItemDBList(list2);
                    break;
                case SubCode.UpgradeEquip:
                    InventoryItemDB itemDB4 = ParameterTool.GetParameter<InventoryItemDB>(request.Parameters, 
                        ParameterCode.InventoryItemDB);
                    Role role = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    peer.LoginRole = role;
                    role.User = peer.LoginUser;
                    itemDB4.Role = role;
                    inventoryItemDBManager.UpgradeEquip(itemDB4, role);
                    break;
            }
        }
    }
}
