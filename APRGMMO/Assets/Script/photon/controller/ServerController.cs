using ARPGCommon;
using ExitGames.Client.Photon;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : ControllerBase
{
    public override void Start()
    {
        base.Start();
        PhotonEngine.Instance.onConnectedToServer += GetServerList;
    }

    public void GetServerList()
    {
        PhotonEngine.Instance.SendRequest(OperationCode.GetServer, new Dictionary<byte, object>());
    }
    public override void OnOperationResponse(OperationResponse response)
    {
        Dictionary<byte, object> parameters = response.Parameters;
        object jsonObject = null;
        parameters.TryGetValue((byte)ParameterCode.ServerList, out jsonObject);
        List<ARPGCommon.Model.ServerProperty> serverList = JsonMapper.
            ToObject<List<ARPGCommon.Model.ServerProperty>>(jsonObject.ToString());

        int index = 0;
        ServerProperty spDefault = null;
        GameObject goDefault = null;
        foreach (ARPGCommon.Model.ServerProperty spTemp in serverList)
        {
            //public string ip = "127.0.0.1:9080";
            //public string Name = "1Çø Ë®Á±¶´";
            //public int count = 120;
            string ip = spTemp.IP + ":4530";
            string name = spTemp.Name;
            int count = spTemp.Count;
            GameObject go;
            if (count > 50)
            {
                //»ð±¬
                go = NGUITools.AddChild(StartMenuController._instance.serverlistGrid.gameObject, StartMenuController._instance.serverItemRed);

            }
            else
            {
                //Á÷³©
                go = NGUITools.AddChild(StartMenuController._instance.serverlistGrid.gameObject, StartMenuController._instance.serverItemGreen);
            }
            ServerProperty sp = go.GetComponent<ServerProperty>();
            sp.ip = ip;
            sp.Name = name;
            sp.count = count;

            if(index == 0)
            {
                spDefault = sp;
                goDefault = go;
            }

            StartMenuController._instance.serverlistGrid.AddChild(go.transform);
            index++;
        }


        StartMenuController.sp = spDefault;
        StartMenuController._instance.servernameLableStart.text = spDefault.Name;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PhotonEngine.Instance.onConnectedToServer -= GetServerList;
    }

    public override OperationCode OpCode
    {
        get { return OperationCode.GetServer; }
    }
}