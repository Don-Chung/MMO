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
            GameController.Instance.transcriptID = btnTranscriptCurrent.id; //���浱ǰ����ĸ�����ID��������н�����㣬���������
            AsyncOperation operation = Application.LoadLevelAsync(btnTranscriptCurrent.sceneName);
            LoadSceneProgressBar._instance.Show(operation);
        }
        else
        {
            MessageManage._instance.ShowMessage("�������㣬���Ժ����");
        }
    }

    public void OnEnterTeam()
    {
        dialog.HideDialog();
        print("OnEnterTeam");
        timerDialog.StartTimer();
        battleController.SendTeam();//������ӵ�����
    }

    //UI�Ļص�
    public void OnCancelTeam()
    {
        battleController.CancelTeam();//��������˷���ȡ����ӵ�����
    }

    //��Ӧ����������ӳɹ�
    public void OnGetTeam(List<Role> roles, int masterRoleID)
    {
        GameController.Instance.battleType = BattleType.Team;
        GameController.Instance.teamRoles = roles;
        if (PhotonEngine.Instance.role.ID == masterRoleID)
        {
            GameController.Instance.isMaster = true;//��ǰ�ͻ����Ƿ�������
        }
        GameController.Instance.transcriptID = btnTranscriptCurrent.id; //���浱ǰ����ĸ�����ID��������н�����㣬���������
        AsyncOperation operation = Application.LoadLevelAsync(btnTranscriptCurrent.sceneName);
        LoadSceneProgressBar._instance.Show(operation);
    }

    //��Ӧ�����������ڵȴ����
    public void OnWaitingTeam()
    {
        print("OnWaitingTeam");
    }

    //��Ӧ��������ȡ����ӳɹ����������˻ص�
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
