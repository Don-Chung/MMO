using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManage : MonoBehaviour
{
    public static TaskManage _instance;

    public TextAsset taskinfoText;
    private ArrayList taskList = new ArrayList();
    private Dictionary<int , Task> taskDict = new Dictionary<int, Task>();
    private Task currentTask;
    private PlayerAutoMove playerAutoMove;
    private PlayerAutoMove PlayerAutoMove
    {
        get
        {
            if(playerAutoMove == null)
            {
                playerAutoMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAutoMove>();
            }
            return playerAutoMove;
        }
    }
    public TaskDBController taskDBController;
    public event OnSyncTaskCompleteEvent OnSyncTaskComplete;

    private void Awake()
    {
        _instance = this;
        taskDBController = this.GetComponent<TaskDBController>();
        taskDBController.OnGetTaskDBList += this.OnGetTaskDBList;
        taskDBController.OnAddTaskDB += this.OnAddTaskDB;
        taskDBController.OnUpdateTaskDB += this.OnUpdateTaskDB;
        InitTask();
        taskDBController.GetTaskDBList();
    }

    public void OnGetTaskDBList(List<TaskDB> list)
    {
        if(list == null)
        {
            return;
        }
        foreach(var taskDB in list)
        {
            Task task = null;
            if(taskDict.TryGetValue(taskDB.TaskID, out task))
            {
                task.SyncTask(taskDB);
            }
        }
        if(OnSyncTaskComplete != null)
        {
            OnSyncTaskComplete();
        }
    }

    public void OnAddTaskDB(TaskDB taskDB)
    {
        Task task = null;
        if (taskDict.TryGetValue(taskDB.TaskID, out task))
        {
            task.SyncTask(taskDB);
        }
    }

    public void OnUpdateTaskDB()
    {

    }

    /// <summary>
    /// 初始化任务信息
    /// </summary>
    public void InitTask()
    {
        string[] taskinfoArray = taskinfoText.ToString().Split('\n');
        foreach (string str in taskinfoArray)
        {
            string[] proArray = str.Split('|');
            Task task = new Task();
            task.Id = int.Parse(proArray[0]);
            switch (proArray[1])
            {
                case "Main":
                    task.TaskType = TaskType.Main;
                    break;
                case "Reward":
                    task.TaskType = TaskType.Reward;
                    break;
                case "Daily":
                    task.TaskType = TaskType.Daily;
                    break;
            }
            task.Name = proArray[2];
            task.Icon = proArray[3];
            task.Des = proArray[4];
            task.Coin = int.Parse(proArray[5]);
            task.Diamond = int.Parse(proArray[6]);
            task.TalkNpc = proArray[7];
            task.IdNpc = int.Parse(proArray[8]);
            task.IdTranscript = int.Parse(proArray[9]);
            taskList.Add(task);
            taskDict.Add(task.Id, task);
        }
    }

    public ArrayList GetTaskList()
    {
        return taskList;
    }

    //执行某个任务
    public void OnExcuteTask(Task task)
    {
        currentTask = task;
        if(task.TaskProgress == TaskProgress.NoStart)
        {
            //导航到NPC处接受任务
            PlayerAutoMove.SetDestination(NPCManage._instance.GetNpcById(task.IdNpc).transform.position);
        }else if(task.TaskProgress == TaskProgress.Accept)
        {
            PlayerAutoMove.SetDestination(NPCManage._instance.transcriptGo.transform.position);
        }
    }

    public void OnAcceptTask()
    {
        currentTask.TaskProgress = TaskProgress.Accept;
        currentTask.UpdateTask(this);//更新任务信息
        //寻路到副本入口
        PlayerAutoMove.SetDestination(NPCManage._instance.transcriptGo.transform.position);
    }
    public void OnArriveDestination()
    {
        if(currentTask == null)
        {
            TranscriptMapUI._instance.Show();
        }
        else
        {
            if (currentTask.TaskProgress == TaskProgress.NoStart)
            {
                //到达NPC处
                NPCDialogUI._instance.Show(currentTask.TalkNpc);
            }else if(currentTask.TaskProgress == TaskProgress.Accept)
            {
                //到副本入口
                TranscriptMapUI._instance.Show();
                TranscriptMapUI._instance.ShowTranscriptEnter(currentTask.IdTranscript);
            }
        }
    }
}
