using ARPGCommon;
using ARPGCommon.Model;
using ARPGCommon.Tools;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDBController : ControllerBase
{
    public override OperationCode OpCode 
    {
        get { return OperationCode.SkillDB; }
    }

    public void Get()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Get, new Dictionary<byte, object>());
    }

    public void Add(SkillDB skillDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        skillDB.Role = null;
        ParameterTool.AddParameter(parameters, ParameterCode.SkillDB, skillDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Add, parameters);
    }

    public void UpdateSkillDB(SkillDB skillDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        skillDB.Role = null;
        ParameterTool.AddParameter(parameters, ParameterCode.SkillDB, skillDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Update, parameters);
    }

    public void Upgrade(SkillDB skillDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        ParameterTool.AddParameter(parameters, ParameterCode.Role, role);
        skillDB.Role = null;
        ParameterTool.AddParameter(parameters, ParameterCode.SkillDB, skillDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.Upgrade, parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetSubCode(response.Parameters);
        switch (subCode)
        {
            case SubCode.Add:
                SkillDB skillDB = ParameterTool.GetParameter<SkillDB>(response.Parameters, ParameterCode.SkillDB);
                if(OnAddSkillDB != null)
                {
                    OnAddSkillDB(skillDB);
                }
                break;
            case SubCode.Get:
                List<SkillDB> list = ParameterTool.GetParameter<List<SkillDB>>(response.Parameters, ParameterCode.SkillDBList);
                if(OnGetSkillDBList != null)
                {
                    OnGetSkillDBList(list);
                }
                break;
            case SubCode.Update:
                if(OnUpdateSkillDB != null)
                {
                    OnUpdateSkillDB();
                }
                break;
            case SubCode.Upgrade:
                SkillDB skillDB2 = ParameterTool.GetParameter<SkillDB>(response.Parameters, ParameterCode.SkillDB);
                if(OnUpgradeSkillDB != null)
                {
                    OnUpgradeSkillDB(skillDB2);
                }
                break;
        }
    }

    public event OnGetSkillDBListEvent OnGetSkillDBList;
    public event OnAddSkillDBEvent OnAddSkillDB;
    public event OnUpdateSkillDBEvent OnUpdateSkillDB;
    public event OnUpgradeSkillDBEvent OnUpgradeSkillDB;
}
