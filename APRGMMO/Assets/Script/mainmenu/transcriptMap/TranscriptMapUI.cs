using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranscriptMapUI : MonoBehaviour
{
    public static TranscriptMapUI _instance;
    private TweenPosition tween;
    private TranscriptMapDialog dialog;
    private Dictionary<int, BtnTranscript> transcriptDict = new Dictionary<int, BtnTranscript>();
    private BtnTranscript btnTranscriptCurrent;
    private BattleController battleController;
    private TimerDialog timerDialog;

    private void Awake()
    {
        _instance = this;
        tween = GetComponent<TweenPosition>();
        dialog = transform.Find("TranscriptMapDialog").GetComponent<TranscriptMapDialog>();
        timerDialog = transform.Find("TimerDialog").GetComponent<TimerDialog>();

        BtnTranscript[] transcripts = this.GetComponentsInChildren<BtnTranscript>();
        foreach (var temp in transcripts)
        {
            transcriptDict.Add(temp.id, temp);
        }
    }

    private void Start()
    {
        battleController = GameController.Instance.GetComponent<BattleController>();
        battleController.OnWaitingTeam += this.OnWaitingTeam;
        battleController.OnCancelTeam += this.OnCancelTeamSuccess;
        battleController.OnGetTeam += this.OnGetTeam;
    }

    public void Show()
    {
        tween.PlayForward();
    }
    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnBack()
    {
        Hide();
    }

    public void OnBtnTranscriptClick(BtnTranscript transcript)
    {
        btnTranscriptCurrent = transcript;
        PlayerInfo info = PlayerInfo._instance;

        if(info.Level >= transcript.needLevel)
        {
            dialog.ShowDialog(transcript);
        }
        else
        {
            dialog.ShowWarn();
        }
    }

    public void ShowTranscriptEnter(int transcriptId)
    {
        BtnTranscript btnTranscript;
        transcriptDict.TryGetValue(transcriptId, out btnTranscript);
        OnBtnTranscriptClick(btnTranscript);
    }

    public void OnEnterPerson()
    {
        if (PlayerInfo._instance.GetEnergy(btnTranscriptCurrent.needEnergy))
        {
            GameController.Instance.battleType = BattleType.Person;
            GameController.Instance.transcriptID = btnTranscriptCurrent.id; //保存当前进入的副本的ID，方便进行结果计算，和任务计算
            AsyncOperation operation = Application.LoadLevelAsync(btnTranscriptCurrent.sceneName);
            LoadSceneProgressBar._instance.Show(operation);
        }
        else
        {
            MessageManage._instance.ShowMessage("体力不足，请稍后进入");
        }
    }

    public void OnEnterTeam()
    {
        dialog.HideDialog();
        print("OnEnterTeam");
        timerDialog.StartTimer();
        battleController.SendTeam();//发起组队的请求
    }

    //UI的回调
    public void OnCancelTeam()
    {
        battleController.CancelTeam();//向服务器端发起取消组队的请求
    }

    //响应服务器端组队成功
    public void OnGetTeam(List<Role> roles, int masterRoleID)
    {
        GameController.Instance.battleType = BattleType.Team;
        GameController.Instance.teamRoles = roles;
        if (PhotonEngine.Instance.role.ID == masterRoleID)
        {
            GameController.Instance.isMaster = true;//当前客户端是否是主机
        }
        GameController.Instance.transcriptID = btnTranscriptCurrent.id; //保存当前进入的副本的ID，方便进行结果计算，和任务计算
        AsyncOperation operation = Application.LoadLevelAsync(btnTranscriptCurrent.sceneName);
        LoadSceneProgressBar._instance.Show(operation);
    }

    //响应服务器端正在等待组队
    public void OnWaitingTeam()
    {
        print("OnWaitingTeam");
    }

    //响应服务器端取消组队成功，服务器端回调
    public void OnCancelTeamSuccess()
    {
        print("OnCancelTeamSuccess");
    }

    void OnDestroy()
    {
        if (battleController != null)
        {
            battleController.OnWaitingTeam -= this.OnWaitingTeam;
            battleController.OnCancelTeam -= this.OnCancelTeamSuccess;
            battleController.OnGetTeam -= this.OnGetTeam;
        }
    }
}
