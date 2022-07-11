using ARPGCommon;
using ARPGCommon.Model;
using ARPGCommon.Tools;
using ExitGames.Client.Photon;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController : ControllerBase
{
    public override OperationCode OpCode 
    { 
        get { return OperationCode.Role; }
    }

    public void GetRole()
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.SubCode, SubCode.GetRole);
        PhotonEngine.Instance.SendRequest(OpCode, parameters);
    }

    public void AddRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.SubCode, SubCode.AddRole);
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode, parameters);
    }

    public void SelectRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SelectRole, parameters);
    }

    public void UpdateRole(Role role)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateRole, parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subCode = ParameterTool.GetParameter<SubCode>(response.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetRole:
                List<Role> list = ParameterTool.GetParameter<List<Role>>(response.Parameters, ParameterCode.RoleList);
                OnGetRole(list);
                break;
            case SubCode.AddRole:
                Role role = ParameterTool.GetParameter<Role>(response.Parameters, ParameterCode.Role);
                OnAddRole(role);
                break;
            case SubCode.SelectRole:
                if(OnSelectRole != null)
                {
                    OnSelectRole();
                }
                break;
        }
    }

    public event OnGetRoleEvent OnGetRole;
    public event OnAddRoleEvent OnAddRole;
    public event OnSelectRoleEvent OnSelectRole;
}
