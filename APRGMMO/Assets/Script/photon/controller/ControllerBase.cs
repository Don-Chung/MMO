using ARPGCommon;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    public abstract OperationCode OpCode { get; }

    public virtual void Start()
    {
        PhotonEngine.Instance.RegisterController(OpCode, this);
    }

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.UnRegisterController(OpCode);
    }

    public virtual void OnEvent(EventData eventData)
    {

    }

    public abstract void OnOperationResponse(OperationResponse response);
}
