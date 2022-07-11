using ARPGCommon;
using ARPGCommon.Model;
using ExitGames.Client.Photon;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterController : ControllerBase
{
    private StartMenuController controller;
    private User user;

    public override OperationCode OpCode 
    { 
      get { return OperationCode.Register; }
    }

    public void Register(string username, string password, StartMenuController controller)
    {
        this.controller = controller;
        user = new User() { Username = username, Password = password};
        string json = JsonMapper.ToJson(user);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.User, json);
        PhotonEngine.Instance.SendRequest(OperationCode.Register, parameters);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        switch (response.ReturnCode)
        {
            case (short)ReturnCode.Fail:
                MessageManage._instance.ShowMessage(response.DebugMessage, 2);
                break;
            case (short)ReturnCode.Success:
                MessageManage._instance.ShowMessage("×¢²á³É¹¦", 2);
                controller.HideRegisterPanel();
                controller.ShowStartPanel();
                controller.usernameLableStart.text = user.Username;
                break;
        }
    }
}
