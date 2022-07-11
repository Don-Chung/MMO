using ARPGCommon;
using ARPGCommon.Model;
using ARPGCommon.Tools;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Battle; }
    }

    public void SendGameState(GameStateModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.GameStateModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SendGameState, parameters);
    }

    //发起请求，同步主角其他动画，比如攻击
    public void SyncPlayerAnimation(PlayerAnimationModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.PlayerAnimationModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncAnimation, parameters);
    }

    /// <summary>
    /// 发送同步动画状态的请求
    /// </summary>
    public void SyncMoveAnimation(PlayerMoveAnimationModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.PlayerMoveAnimationModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncMoveAnimation, parameters);
    }

    //发起同步位置和旋转的请求
    public void SyncPositionAndRotation(Vector3 position, Vector3 eulerAngles)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.RoleID, PhotonEngine.Instance.role.ID, false);
        ParameterTool.AddParameter(parameters, ParameterCode.Position, new Vector3Obj(position));
        ParameterTool.AddParameter(parameters, ParameterCode.EulerAngles, new Vector3Obj(eulerAngles));
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncPositionAndRotation, parameters);
    }

    //发起组队的请求
    public void SendTeam()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SendTeam, new Dictionary<byte, object>());
    }

    public void CancelTeam()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.CancelTeam, new Dictionary<byte, object>());
    }

    public override void OnEvent(EventData eventData)
    {
        SubCode subCode = ParameterTool.GetSubCode(eventData.Parameters);
        switch (subCode)
        {
            case SubCode.GetTeam:
                List<Role> roles = ParameterTool.GetParameter<List<Role>>(eventData.Parameters,
                        ParameterCode.RoleList);
                int masterRoleID = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.MasterRoleID,
                    false);
                if (OnGetTeam != null)
                {
                    OnGetTeam(roles, masterRoleID);
                }
                break;
            case SubCode.SyncPositionAndRotation:
                int roleID = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleID, false);
                Vector3 pos =
                    ParameterTool.GetParameter<Vector3Obj>(eventData.Parameters, ParameterCode.Position).ToVector3();
                Vector3 eulerAngles = ParameterTool.GetParameter<Vector3Obj>(eventData.Parameters,
                    ParameterCode.EulerAngles).ToVector3();
                if (OnSyncPositionAndRotation != null)
                {
                    OnSyncPositionAndRotation(roleID, pos, eulerAngles);
                }
                break;
            case SubCode.SyncMoveAnimation:
                int roleID2 = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleID, false);
                PlayerMoveAnimationModel model =
                    ParameterTool.GetParameter<PlayerMoveAnimationModel>(eventData.Parameters,
                        ParameterCode.PlayerMoveAnimationModel);
                if (OnSyncMoveAnimation != null)
                {
                    OnSyncMoveAnimation(roleID2, model);
                }
                break;
            case SubCode.SyncAnimation:
                int roleID3 = ParameterTool.GetParameter<int>(eventData.Parameters, ParameterCode.RoleID, false);
                PlayerAnimationModel model2 = ParameterTool.GetParameter<PlayerAnimationModel>(eventData.Parameters,
                    ParameterCode.PlayerAnimationModel);
                if (OnSyncPlayerAnimation != null)
                {
                    OnSyncPlayerAnimation(roleID3, model2);
                }
                break;
            case SubCode.SendGameState:
                GameStateModel model3 = ParameterTool.GetParameter<GameStateModel>(eventData.Parameters,
                    ParameterCode.GameStateModel);
                if (OnGameStateChange != null)
                {
                    OnGameStateChange(model3);
                }
                break;
        }
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        SubCode subcode = ParameterTool.GetSubCode(response.Parameters);
        switch (subcode)
        {
            case SubCode.SendTeam:
                if (response.ReturnCode == (int)ReturnCode.GetTeam)
                {
                    List<Role> roles = ParameterTool.GetParameter<List<Role>>(response.Parameters,
                        ParameterCode.RoleList);
                    int masterRoleID = ParameterTool.GetParameter<int>(response.Parameters, ParameterCode.MasterRoleID,
                        false);
                    if (OnGetTeam != null)
                    {
                        OnGetTeam(roles, masterRoleID);
                    }
                }
                else if (response.ReturnCode == (int)ReturnCode.WaitingTeam)
                {
                    if (OnWaitingTeam != null)
                    {
                        OnWaitingTeam();
                    }
                }
                break;
            case SubCode.CancelTeam:
                if (OnCancelTeam != null)
                {
                    OnCancelTeam();
                }
                break;
        }
    }

    public event OnGetTeamEvent OnGetTeam;
    public event OnWaitingTeamEvent OnWaitingTeam;
    public event OnCancelTeamEvent OnCancelTeam;
    public event OnSyncPositionAndRotationEvent OnSyncPositionAndRotation;
    public event OnSyncMoveAnimationEvent OnSyncMoveAnimation;
    public event OnSyncPlayerAnimationEvent OnSyncPlayerAnimation;
    public event OnGameStateChangeEvent OnGameStateChange;
}
