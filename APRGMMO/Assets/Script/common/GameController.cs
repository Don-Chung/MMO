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
    public HashSet<int> dieRoleIDSet = new HashSet<int>(); //�洢�����Ѿ������Ľ�ɫ��id
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
    {   //�Ȳ�����
        return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2f);
    }

    public int GetRandomRoleID()//��ȡһ������Ľ�ɫid
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
            GameOverPanel.Instance.Show("��Ϸʧ��");
        }
        else
        {
            if (isMaster)//ֻ����������ʧ�ܺ�ʤ���ļ��
            {
                dieRoleIDSet.Add(roleID);
                if (dieRoleIDSet.Count == teamCount)
                {
                    GameOverPanel.Instance.Show("��Ϸʧ��");
                    //�������ͻ��˷���Ϸʧ�ܵ�״̬
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
                // �������ͻ��˷�����Ϸʤ����״̬
                battleController.SendGameState(new GameStateModel() { isSuccess = true });
            }
        }
    }

    void OnVictory()
    {
        GameOverPanel.Instance.Show("��Ϸʤ��");
        foreach (Task task in TaskManage._instance.GetTaskList())
        {
            if (task.TaskProgress == TaskProgress.Accept)
            {
                if (task.IdTranscript == transcriptID)
                {
                    task.TaskProgress = TaskProgress.Reward; //�޸�����״̬Ϊ��ȡ����״̬
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
            GameOverPanel.Instance.Show("��Ϸʧ��");
        }
    }

    /// <summary>
    /// ��ӳɹ�
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
            Debug.LogWarning("δ�ҵ���Ӧ�Ľ�ɫ��Ϸ������и���");
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
            Debug.LogWarning("δ�ҵ���Ӧ�Ľ�ɫ��Ϸ������и����ƶ�����״̬");
        }
    }

    //���ݽ�ɫid��ͬ������
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
            Debug.LogWarning("δ�ҵ���Ӧ�Ľ�ɫ��Ϸ������и��¶���״̬");
        }
    }

}
