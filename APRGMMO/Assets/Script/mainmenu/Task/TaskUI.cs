using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    public static TaskUI _instance;
    private UIGrid taskListGrid;
    private TweenPosition tween;
    private UIButton closeButton;
    public GameObject taskItemPrefab;

    private void Awake()
    {
        _instance = this;
        taskListGrid = transform.Find("Scroll View/Grid").GetComponent<UIGrid>();
        tween = this.GetComponent<TweenPosition>();
        closeButton = transform.Find("CloseButton").GetComponent<UIButton>();

        EventDelegate ed = new EventDelegate(this, "OnClose");
        closeButton.onClick.Add(ed);
    }

    private void Start()
    {
        TaskManage._instance.OnSyncTaskComplete += this.OnSyncTaskComplete;
    }

    public void OnSyncTaskComplete()
    {
        InitTaskList();
    }

    /// <summary>
    /// 初始化任务列表信息
    /// </summary>
    void InitTaskList()
    {
        ArrayList taskList = TaskManage._instance.GetTaskList();

        foreach(Task task in taskList)
        {
            GameObject go =  NGUITools.AddChild(taskListGrid.gameObject, taskItemPrefab);
            taskListGrid.AddChild(go.transform);
            TaskItemUI ti = go.GetComponent<TaskItemUI>();
            ti.SetTask(task);
        }
    }

    public void Show()
    {
        tween.PlayForward();
    }
    public void Hide()
    {
        tween.PlayReverse();
    }
    private void OnClose()
    {
        Hide();
    }
}
