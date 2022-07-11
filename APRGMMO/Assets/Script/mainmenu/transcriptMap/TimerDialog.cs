using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDialog : MonoBehaviour
{
    private UILabel stateLabel;
    private UILabel timeLabel;
    private UIButton cancelButton;
    private TweenScale tween;
    public float time = 10;
    private float timer = 0;
    private bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        stateLabel = transform.Find("StateLabel").GetComponent<UILabel>();
        timeLabel = transform.Find("TimeLabel").GetComponent<UILabel>();
        cancelButton = transform.Find("CancelButton").GetComponent<UIButton>();
        tween = GetComponent<TweenScale>();
        EventDelegate ed = new EventDelegate(this, "OnCancelButtonClick");
        cancelButton.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timer += Time.deltaTime;
            int remainTime = (int)(time - timer);
            timeLabel.text = remainTime.ToString();
            if (timer > time)
            {
                timer = 0;
                isStart = false;
                OnTimeEnd();
            }
        }
    }

    public void StartTimer()//显示组队计时器
    {
        tween.PlayForward();
        timer = 0;
        isStart = true;
    }

    void HideTimer()
    {
        tween.PlayReverse();
        isStart = false;
    }

    void OnCancelButtonClick()
    {
        HideTimer();
        transform.parent.SendMessage("OnCancelTeam");
    }

    void OnTimeEnd()//计时结束
    {
        HideTimer();
        transform.parent.SendMessage("OnCancelTeam");
    }
}
