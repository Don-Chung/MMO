using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    public static SkillUI _instance;
    private UILabel skillNameLabel;
    private UILabel skillDesLabel;
    private UIButton closeButton;
    private UIButton upgradeButton;
    private UILabel upgradeButtonLabel;
    private TweenPosition tween;
    private Skill skill;

    private void Awake()
    {
        _instance = this;
        skillNameLabel = transform.Find("Bg/SkillNameLabel").GetComponent<UILabel>();
        skillDesLabel = transform.Find("Bg/DesLabel").GetComponent<UILabel>();
        closeButton = transform.Find("CloseButton").GetComponent<UIButton>();
        upgradeButton = transform.Find("UpgradeButton").GetComponent<UIButton>();
        upgradeButtonLabel = transform.Find("UpgradeButton/Label").GetComponent<UILabel>();
        tween = GetComponent<TweenPosition>();

        skillNameLabel.text = "";
        skillDesLabel.text = "";
        DisabelUpgradeButton("ѡ����");

        EventDelegate ed = new EventDelegate(this, "OnUpgrade");
        upgradeButton.onClick.Add(ed);

        EventDelegate ed1 = new EventDelegate(this, "OnClose");
        closeButton.onClick.Add(ed1);
    }

    void DisabelUpgradeButton(string label = "")
    {
        upgradeButton.SetState(UIButton.State.Disabled, true);
        upgradeButton.GetComponent<Collider>().enabled = false;
        if(label != "")
        {
            upgradeButtonLabel.text = label;
        }
    }

    void EnableUpgradeButton(string label = "")
    {
        upgradeButton.SetState(UIButton.State.Normal, true);
        upgradeButton.GetComponent<Collider>().enabled = true;
        if (label != "")
        {
            upgradeButtonLabel.text = label;
        }
    }

    void OnSkillClick(Skill skill)
    {
        this.skill = skill;
        PlayerInfo info = PlayerInfo._instance;
        if(500 * (skill.Level + 1) <= info.Coin)
        {
            if (skill.Level < info.Level)
                EnableUpgradeButton("����");
            else
                DisabelUpgradeButton("���ȼ�");
        }
        else
        {
            DisabelUpgradeButton("��Ҳ���");
        }
        skillNameLabel.text = skill.Name.ToString() + " Lv." + skill.Level.ToString();
        skillDesLabel.text = "��ǰ���ܹ�������" + (skill.Damage * skill.Level).ToString()
            + " ��һ�ȼ���������" + (skill.Damage * (skill.Level + 1)).ToString()
            + " ��������������" + (500 * (skill.Level + 1)).ToString();
    }

    void OnUpgrade()
    {
        PlayerInfo info = PlayerInfo._instance;
        if(skill.Level < info.Level)
        {
            int coinNeed = 500 * (skill.Level + 1);
            bool isSuccess = info.GetCoin(coinNeed);
            if (isSuccess)
            {
                skill.Upgrade();
                //ͬ�������ݿ�skilldb,role
                SkillManage._instance.Upgrade(skill);
                OnSkillClick(skill);
            }
            else
            {
                DisabelUpgradeButton("��Ҳ���");
            }
        }
        else
        {
            DisabelUpgradeButton("���ȼ�");
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
    public void OnClose()
    {
        Hide();
    }
}
