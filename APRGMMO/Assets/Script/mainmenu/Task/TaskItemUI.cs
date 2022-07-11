using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskItemUI : MonoBehaviour
{
    private UISprite taskTypeSprite;
    private UISprite iconSprite;
    private UILabel nameLabel;
    private UILabel desLabel;
    private UISprite reward1Sprite;
    private UILabel reward1Label;
    private UISprite reward2Sprite;
    private UILabel reward2Label;
    private UIButton rewardButton;
    private UIButton combatButton;
    private UILabel combatButtonLabel;

    private Task task;

    private void Awake()
    {
        taskTypeSprite = transform.Find("TaskTypeSprite").GetComponent<UISprite>();
        iconSprite = transform.Find("IconBg/Sprite").GetComponent<UISprite>();
        nameLabel = transform.Find("NameLabel").GetComponent<UILabel>();
        desLabel = transform.Find("DesLabel").GetComponent<UILabel>();
        reward1Sprite = transform.Find("Reward1Sprite").GetComponent<UISprite>();
        reward1Label = transform.Find("Reward1Sprite/Label").GetComponent<UILabel>();
        reward2Sprite = transform.Find("Reward2Sprite").GetComponent<UISprite>();
        reward2Label = transform.Find("Reward2Sprite/Label").GetComponent<UILabel>();
        rewardButton = transform.Find("RewardButton").GetComponent<UIButton>();
        combatButton = transform.Find("CombatButton").GetComponent<UIButton>();
        combatButtonLabel = transform.Find("CombatButton/Label").GetComponent<UILabel>();

        EventDelegate ed1 = new EventDelegate(this, "OnCombat");
        combatButton.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnReward");
        rewardButton.onClick.Add(ed2);
    }

    public void SetTask(Task task)
    {
        this.task = task;
        task.onTaskChange += this.OnTaskChange;
        UpdateShow();
    }

    void UpdateShow()
    {
        switch (task.TaskType)
        {
            case TaskType.Main:
                taskTypeSprite.spriteName = "pic_主线";
                break;
            case TaskType.Reward:
                taskTypeSprite.spriteName = "pic_奖赏";
                break;
            case TaskType.Daily:
                taskTypeSprite.spriteName = "pic_日常";
                break;
        }
        iconSprite.spriteName = task.Icon;
        nameLabel.text = task.Name;
        desLabel.text = task.Des;
        if (task.Coin > 0 && task.Diamond > 0)
        {
            reward1Sprite.spriteName = "金币";
            reward1Label.text = "x" + task.Coin.ToString();
            reward2Sprite.spriteName = "钻石";
            reward2Label.text = "x" + task.Diamond.ToString();
        }
        else if (task.Coin > 0)
        {
            reward1Sprite.spriteName = "金币";
            reward1Label.text = "x" + task.Coin.ToString();
            reward2Sprite.gameObject.SetActive(false);
            reward2Label.gameObject.SetActive(false);
        }
        else
        {
            reward1Sprite.gameObject.SetActive(false);
            reward1Label.gameObject.SetActive(false);
            reward2Sprite.spriteName = "钻石";
            reward2Label.text = "x" + task.Diamond.ToString();
        }
        switch (task.TaskProgress)
        {
            case TaskProgress.NoStart:
                rewardButton.gameObject.SetActive(false);
                combatButtonLabel.text = "下一步";
                break;
            case TaskProgress.Accept:
                rewardButton.gameObject.SetActive(false);
                combatButtonLabel.text = "战斗";
                break;
            case TaskProgress.Complete:
                combatButtonLabel.gameObject.SetActive(false);
                rewardButton.gameObject.SetActive(true);
                break;
        }
    }

    void OnCombat()
    {
        TaskUI._instance.Hide();
        TaskManage._instance.OnExcuteTask(task);
    }
    void OnReward()
    {

    }
    void OnTaskChange()
    {
        UpdateShow();
    }
}
