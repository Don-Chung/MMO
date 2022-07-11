using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleType
{
    Person,
    Team,
    None
}

public class GameController : MonoBehaviour
{
    public static GameController _instance;
    public static GameController Instance
    {
        get { return _instance; }
    }
    public BattleType battleType = BattleType.None;
    public List<Role> teamRoles = new List<Role>();
    public HashSet<int> dieRoleIDSet = new HashSet<int>(); //存储所有已经死亡的角色的id
    public int transcriptID = -1;
    public bool isMaster = false;

    private Dictionary<int, GameObject> playerDict = new Dictionary<int, GameObject>();
    private TaskDBController taskDBController;
    private BattleController battleController;
    private int teamCount;
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        Transform PosTranfrom = GameObject.Find("PlayerPos").transform;
        string playerPrefabName = "Player_girl";
        if (PhotonEngine.Instance.role.Isman)
        {
            playerPrefabName = "Player_boy";
        }
        GameObject playerGo = GameObject.Instantiate(Resources.Load("Player/" + playerPrefabName)) as GameObject;
        playerGo.transform.position = PosTranfrom.position;
        taskDBController = GetComponent<TaskDBController>();
        battleController = GetComponent<BattleController>();
        battleController.OnGetTeam += this.OnGetTeam;
        battleController.OnSyncPositionAndRotation += this.OnSyncPositionAndRotation;
        battleController.OnSyncMoveAnimation += this.OnSyncMoveAnimation;
        battleController.OnSyncPlayerAnimation += this.OnSyncPlayerAnimation;
        battleController.OnGameStateChange += OnGameStateChange;
    }

    private void Start()
    {
        teamCount = teamRoles.Count;
    }

    public static int GetRequireExpByLevel(int level)
    {   //等差数列
        return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2f);
    }

    public int GetRandomRoleID()//获取一个随机的角色id
    {
        if (battleType == BattleType.Team)
        {
            int index = Random.Range(0, teamRoles.Count);
            return teamRoles[index].ID;
        }
        else
        {
            return PhotonEngine.Instance.role.ID;
        }
    }

    public GameObject GetPlayerByRoleID(int roleID)
    {
        if (battleType == BattleType.Team)
        {
            GameObject go = null;
            playerDict.TryGetValue(roleID, out go);
            return go;
        }
        else
        {
            return TranscriptManage._instance.player.gameObject;
        }
    }

    public void OnPlayerDie(int roleID)
    {
        if (battleType == BattleType.Person)
        {
            GameOverPanel.Instance.Show("游戏失败");
        }
        else
        {
            if (isMaster)//只在主机端做失败和胜利的检测
            {
                dieRoleIDSet.Add(roleID);
                if (dieRoleIDSet.Count == teamCount)
                {
                    GameOverPanel.Instance.Show("游戏失败");
                    //向其他客户端发游戏失败的状态
                    battleController.SendGameState(new GameStateModel() { isSuccess = false });
                }
            }
        }
    }

    public void OnBossDie()
    {
        if (battleType == BattleType.Person)
        {
            OnVictory();
        }
        else
        {
            if (isMaster)
            {
                OnVictory();
                // 向其他客户端发送游戏胜利的状态
                battleController.SendGameState(new GameStateModel() { isSuccess = true });
            }
        }
    }

    void OnVictory()
    {
        GameOverPanel.Instance.Show("游戏胜利");
        foreach (Task task in TaskManage._instance.GetTaskList())
        {
            if (task.TaskProgress == TaskProgress.Accept)
            {
                if (task.IdTranscript == transcriptID)
                {
                    task.TaskProgress = TaskProgress.Reward; //修改任务状态为领取奖励状态
                    TaskDB taskDB = task.TaskDB;
                    taskDB.State = (int)TaskState.Reward;
                    taskDBController.UpdateTaskDB(taskDB);
                }
            }
        }
    }
    void OnGameStateChange(GameStateModel model)
    {
        if (model.isSuccess)
        {
            OnVictory();
        }
        else
        {
            GameOverPanel.Instance.Show("游戏失败");
        }
    }

    /// <summary>
    /// 组队成功
    /// </summary>
    public void OnGetTeam(List<Role> roles, int masterRoleID)
    {
        
    }

    void OnDestroy()
    {
        battleController.OnGetTeam -= OnGetTeam;
    }

    public void AddPlayer(int roleID, GameObject playerGameObject)
    {
        playerDict.Add(roleID, playerGameObject);
    }

    public void OnSyncPositionAndRotation(int roleID, Vector3 position, Vector3 eulerAngles)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleID, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove>().SetPositionAndRotation(position, eulerAngles);
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新");
        }
    }

    public void OnSyncMoveAnimation(int roleID, PlayerMoveAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleID, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove>().SetAnim(model);
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新移动动画状态");
        }
    }

    //根据角色id来同步动画
    public void OnSyncPlayerAnimation(int roleID, PlayerAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleID, out go);
        if (isHave)
        {
            go.GetComponent<PlayerAnimation>().SyncAnimation(model);
            if (model.die)
            {
                OnPlayerDie(roleID);
            }
        }
        else
        {
            Debug.LogWarning("未找到对应的角色游戏物体进行更新动画状态");
        }
    }

}
