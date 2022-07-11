using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Main = 0, //����
    Reward = 1, //�ͽ�
    Daily = 2//�ճ�
}

public enum TaskProgress
{
    NoStart = 0,
    Accept = 1,
    Complete = 2,
    Reward = 3
}

public class Task
{
    private int id;
    private TaskType taskType;
    private string name;
    private string icon;
    private string des;
    private int coin;
    private int diamond;
    private string talkNpc;
    private int idNpc;
    private int idTranscript;
    private TaskProgress taskProgress = TaskProgress.NoStart;

    public TaskDB TaskDB { get; set; } //��Ӧ�ͻ����ڷ������˴洢���������

    public delegate void OnTaskChangeEvent();
    public event OnTaskChangeEvent onTaskChange;

    //����ͬ��������Ϣ
    public void SyncTask(TaskDB taskDb)
    {
        TaskDB = taskDb;
        taskProgress = (TaskProgress)taskDb.State;
    }

    public void UpdateTask(TaskManage taskManage)
    {
        if(TaskDB == null)
        {
            TaskDB = new TaskDB();
            TaskDB.State = (int)TaskProgress;
            TaskDB.TaskID = id;
            TaskDB.LastUpdateTime = new System.DateTime();
            TaskDB.Type = (int)(ARPGCommon.Model.TaskType)taskType;
            taskManage.taskDBController.AddTaskDB(TaskDB);
        }
        else
        {
            this.TaskDB.State = (int)TaskProgress;
            taskManage.taskDBController.UpdateTaskDB(this.TaskDB);
        }
    }

    public int Id { get => id; set => id = value; }
    public TaskType TaskType { get => taskType; set => taskType = value; }
    public string Name { get => name; set => name = value; }
    public string Icon { get => icon; set => icon = value; }
    public string Des { get => des; set => des = value; }
    public int Coin { get => coin; set => coin = value; }
    public int Diamond { get => diamond; set => diamond = value; }
    public string TalkNpc { get => talkNpc; set => talkNpc = value; }
    public int IdNpc { get => idNpc; set => idNpc = value; }
    public int IdTranscript { get => idTranscript; set => idTranscript = value; }
    public TaskProgress TaskProgress
    {
        get { return taskProgress; }
        set {
            if (taskProgress != value)
            {
                taskProgress = value;
                onTaskChange();
            }
        }
    }
}
