using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Main, //主线
    Reward, //赏金
    Daily //日常
}

public enum TaskProgress
{
    NoStart,
    Accept,
    Complete,
    Reward
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
    public TaskProgress TaskProgress { get => taskProgress; set => taskProgress = value; }
}
