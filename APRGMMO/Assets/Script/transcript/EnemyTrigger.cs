using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPosArray;
    public float time = 0;//多少秒后开始生成
    public float repeatRate = 4;
    private bool isSpawn = false;
    private EnemyController enemyController;

    private void Start()
    {
        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            enemyController = TranscriptManage._instance.GetComponent<EnemyController>();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (GameController.Instance.battleType == BattleType.Person)
        {
            if (col.tag == "Player" && isSpawn == false)
            {
                isSpawn = true;
                StartCoroutine(SpawnEnemy());
            }
        }
        else if (GameController.Instance.battleType == BattleType.Team)
        {
            if (col.tag == "Player" && isSpawn == false && GameController.Instance.isMaster)
            {//当是团队战斗模式的时候，判断当前客户端是否是主机，是主机的话才会生成敌人
                isSpawn = true;
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(time);
        //发消息 让其他客户端产生相应的敌人 （位置，敌人类型）
        foreach (GameObject go in enemyPrefabs)
        {
            List<CreateEnemyProperty> propertyList = new List<CreateEnemyProperty>();
            foreach (Transform t in spawnPosArray)
            {
                GameObject temp = GameObject.Instantiate(go, t.position, Quaternion.identity) as GameObject;
                string GUID = Guid.NewGuid().ToString();
                int targetRoleId = GameController.Instance.GetRandomRoleID();
                if (temp.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = temp.GetComponent<Enemy>();
                    enemy.guid = GUID; //为每一个新生成的敌人创建一个GUID
                    enemy.targetRoleId = targetRoleId;
                }
                else
                {
                    Boss boss = temp.GetComponent<Boss>();
                    boss.guid = GUID;
                    boss.targetRoleId = targetRoleId;
                }

                CreateEnemyProperty property = new CreateEnemyProperty()
                {
                    guid = GUID,
                    position = new Vector3Obj(t.position),
                    prefabName = go.name,
                    targetRoleID = targetRoleId

                };
                propertyList.Add(property);
            }

            if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster) //在团队模式下，且是主机时才发送
            {
                CreateEnemyModel model = new CreateEnemyModel() { list = propertyList };
                enemyController.SendCreateEnemy(model);
            }

            yield return new WaitForSeconds(repeatRate);
        }
    }
}
