using ARPGCommon;
using ARPGCommon.Tools;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ControllerBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Enemy; }
    }

    //发起创建敌人的请求
    public void SendCreateEnemy(CreateEnemyModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.CreateEnemyModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.CreateEnemy, parameters);
    }

    //用来同步敌人的位置和旋转
    public void SyncEnemyPosition(EnemyPositionModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.EnemyPositionModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncPositionAndRotation, parameters);
    }
    //发起敌人动画同步的请求
    public void SyncEnemyAnimation(EnemyAnimationModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.EnemyAnimationModel, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncAnimation, parameters);
    }

    public override void OnEvent(EventData eventData)
    {
        SubCode subcode = ParameterTool.GetSubCode(eventData.Parameters);
        switch (subcode)
        {
            case SubCode.CreateEnemy:
                CreateEnemyModel model = ParameterTool.GetParameter<CreateEnemyModel>(eventData.Parameters,
                    ParameterCode.CreateEnemyModel);
                if (OnCreateEnemy != null)
                {
                    OnCreateEnemy(model);
                }
                break;
            case SubCode.SyncPositionAndRotation:
                EnemyPositionModel model1 = ParameterTool.GetParameter<EnemyPositionModel>(eventData.Parameters,
                    ParameterCode.EnemyPositionModel);
                if (OnSyncEnemyPositionAndRotation != null)
                {
                    OnSyncEnemyPositionAndRotation(model1);
                }
                break;
            case SubCode.SyncAnimation:
                EnemyAnimationModel model2 = ParameterTool.GetParameter<EnemyAnimationModel>(eventData.Parameters,
                    ParameterCode.EnemyAnimationModel);
                if (OnSyncEnemyAnimation != null)
                {
                    OnSyncEnemyAnimation(model2);
                }
                break;
        }
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        
    }

    public event OnCreateEnemyEvent OnCreateEnemy;
    public event OnSyncEnemyPositionRotationEvent OnSyncEnemyPositionAndRotation;
    public event OnSyncEnemyAnimationEvent OnSyncEnemyAnimation;
}
