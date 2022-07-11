using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranscriptManage : MonoBehaviour
{
    public static TranscriptManage _instance;


    public GameObject player;//表示当前的主角

    private List<GameObject> enemyList = new List<GameObject>();
    private Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();
    private List<Enemy> enemyToSyncList = new List<Enemy>();//需要同步的敌人的集合
    private Boss bossToSync = null;
    private List<Enemy> enemyToSyncAnimationList = new List<Enemy>();
    private EnemyController enemyController;

    private void Awake()
    {
        _instance = this;
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (GameController.Instance.battleType == BattleType.Team)
        {

            enemyController = GetComponent<EnemyController>();
            enemyController.OnCreateEnemy += this.OnCreateEnemy;
            enemyController.OnSyncEnemyPositionAndRotation += this.OnSyncEnemyPositionAndRotation;
            enemyController.OnSyncEnemyAnimation += this.OnSyncEnemyAnimation;
        }

        if (GameController.Instance.battleType == BattleType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("SyncEnemyPositionAndRotation", 0, 1 / 30f);
            InvokeRepeating("SyncEnemyAnimation", 0, 1 / 30f);
        }
    }

    void OnDestroy()
    {
        if (enemyController != null)
        {
            enemyController.OnCreateEnemy -= this.OnCreateEnemy;
        }
    }

    public void OnCreateEnemy(CreateEnemyModel model)
    {
        foreach (CreateEnemyProperty property in model.list)
        {
            GameObject enemyPrefab = Resources.Load("enemy/" + property.prefabName) as GameObject;
            GameObject go = GameObject.Instantiate(enemyPrefab, property.position.ToVector3(), Quaternion.identity) as GameObject;
            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.guid = property.guid;
                enemy.targetRoleId = property.targetRoleID;
            }
            else
            {
                Boss boss = go.GetComponent<Boss>();
                boss.guid = property.guid;
                boss.targetRoleId = property.targetRoleID;
            }
        }
    }


    public void AddEnemy(GameObject enemyGo)
    {
        enemyList.Add(enemyGo);
        string guid = null;
        if (enemyGo.GetComponent<Enemy>() != null)
        {
            guid = enemyGo.GetComponent<Enemy>().guid;
        }
        else
        {
            guid = enemyGo.GetComponent<Boss>().guid;
        }
        enemyDict.Add(guid, enemyGo);
    }

    public void RemoveEnemy(GameObject enemyGo)
    {
        enemyList.Remove(enemyGo);
        string guid = null;
        if (enemyGo.GetComponent<Enemy>() != null)
        {
            guid = enemyGo.GetComponent<Enemy>().guid;
        }
        else
        {
            guid = enemyGo.GetComponent<Boss>().guid;
        }
        enemyDict.Remove(guid);
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    public Dictionary<string, GameObject> GetEnemyDict()
    {
        return enemyDict;
    }

    public void AddEnemyToSync(Enemy enemy)
    {
        enemyToSyncList.Add(enemy);
    }

    public void AddBossToSync(Boss boss)
    {
        this.bossToSync = boss;
    }

    public void AddEnemyToSyncAnimation(Enemy enemy)
    {
        enemyToSyncAnimationList.Add(enemy);
    }

    //发起请求
    void SyncEnemyPositionAndRotation()
    {

        if (enemyToSyncList != null && enemyToSyncList.Count > 0)
        {
            EnemyPositionModel model = new EnemyPositionModel();
            foreach (Enemy enemy in enemyToSyncList)
            {
                if (enemy != null) //可能同步的时候敌人已经死了
                {
                    EnemyPositionProperty property = new EnemyPositionProperty()
                    {
                        guid = enemy.guid,
                        position = new Vector3Obj(enemy.transform.position),
                        eulerAngles = new Vector3Obj(enemy.transform.eulerAngles)
                    };
                    model.list.Add(property);
                }
            }
            if (bossToSync != null)
            {
                EnemyPositionProperty property = new EnemyPositionProperty()
                {
                    guid = bossToSync.guid,
                    position = new Vector3Obj(bossToSync.transform.position),
                    eulerAngles = new Vector3Obj(bossToSync.transform.eulerAngles)
                };
                model.list.Add(property);
            }
            bossToSync = null;
            enemyController.SyncEnemyPosition(model);
            enemyToSyncList.Clear();
        }
    }
    //接受响应
    void OnSyncEnemyPositionAndRotation(EnemyPositionModel model)
    {
        foreach (EnemyPositionProperty property in model.list)
        {
            GameObject enemyGo;
            bool isGet = enemyDict.TryGetValue(property.guid, out enemyGo);
            if (isGet)
            {
                enemyGo.transform.position = property.position.ToVector3();
                enemyGo.transform.eulerAngles = property.eulerAngles.ToVector3();
            }
        }
    }
    //用来发起敌人动画同步的请求
    void SyncEnemyAnimation()
    {
        if (enemyToSyncAnimationList != null && enemyToSyncAnimationList.Count > 0)
        {
            EnemyAnimationModel model = new EnemyAnimationModel();
            foreach (Enemy enemy in enemyToSyncAnimationList)
            {
                Animation anim = enemy.GetComponent<Animation>();
                EnemyAnimationProperty property = new EnemyAnimationProperty()
                {
                    guid = enemy.guid,
                    isAttack = anim.IsPlaying("attack01"),
                    isDie = anim.IsPlaying("die"),
                    isIdle = anim.IsPlaying("idle"),
                    isTakeDamage = anim.IsPlaying("takedamage"),
                    isWalk = anim.IsPlaying("walk")
                };
                model.list.Add(property);
            }
            enemyController.SyncEnemyAnimation(model);
            enemyToSyncAnimationList.Clear();
        }
    }

    void OnSyncEnemyAnimation(EnemyAnimationModel model)
    {
        foreach (EnemyAnimationProperty property in model.list)
        {
            GameObject enemyGo;
            bool isGet = enemyDict.TryGetValue(property.guid, out enemyGo);
            if (isGet)
            {
                Animation anim = enemyGo.GetComponent<Animation>();
                if (property.isIdle)
                {
                    anim.Play("idle");
                }
                if (property.isAttack)
                {
                    anim.Play("attack01");
                }
                if (property.isDie)
                {
                    anim.Play("die");
                }
                if (property.isTakeDamage)
                {
                    anim.Play("takedamage");
                }
                if (property.isWalk)
                {
                    anim.Play("walk");
                }
            }

        }
    }

}
