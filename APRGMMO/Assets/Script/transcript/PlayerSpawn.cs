using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform[] positionTransformArray;

    private void Awake()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (GameController.Instance.battleType == BattleType.Person)
        {
            //个人战斗角色加载
            Role role = PhotonEngine.Instance.role;
            GameObject playerPrefab;
            if (role.Isman)
            {
                playerPrefab = Resources.Load("player_battle/Player_boy") as GameObject;
            }
            else
            {
                playerPrefab = Resources.Load("player_battle/Player_girl") as GameObject;
            }
            GameObject go = GameObject.Instantiate(playerPrefab, positionTransformArray[0].position, Quaternion.identity) as GameObject;
            TranscriptManage._instance.player = go;
            go.GetComponent<Player>().roleID = role.ID;
        }
        else if (GameController.Instance.battleType == BattleType.Team)
        {
            //团队战斗角色加载
            for (int i = 0; i < 3; i++)
            {
                Role role = GameController.Instance.teamRoles[i];
                Vector3 pos = positionTransformArray[i].position;
                GameObject playerPrefab;
                if (role.Isman)
                {
                    playerPrefab = Resources.Load("player_battle/Player_boy") as GameObject;
                }
                else
                {
                    playerPrefab = Resources.Load("player_battle/Player_girl") as GameObject;
                }
                GameObject go = GameObject.Instantiate(playerPrefab, pos, Quaternion.identity) as GameObject;
                go.GetComponent<Player>().roleID = role.ID;
                GameController.Instance.AddPlayer(role.ID, go);
                if (role.ID == PhotonEngine.Instance.role.ID)
                {
                    //当前创建的角色是当前客户端控制的
                    TranscriptManage._instance.player = go;
                }
                else
                {
                    //这个角色是其他客户端的  修改移动为不可控
                    go.GetComponent<PlayerMove>().isCanControl = false;
                }
            }
        }
    }
}
