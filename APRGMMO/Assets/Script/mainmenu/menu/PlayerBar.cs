using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    private UISprite headSprite;
    private UILabel nameLabel;
    private UILabel levelLabel;
    private UISlider energySlider;
    private UILabel energyLabel;
    private UISlider toughenSlider;
    private UILabel toughenLabel;
    private UIButton energyPlusBotton;
    private UIButton toughenPlusBotton;

    private UIButton headButton;

    private void Awake()
    {
        headSprite = transform.Find("HeadSprite").GetComponent<UISprite>();
        nameLabel = transform.Find("NameLabel").GetComponent<UILabel>();
        levelLabel = transform.Find("LevelLabel").GetComponent<UILabel>();
        energySlider = transform.Find("EnergyProgressBar").GetComponent<UISlider>();
        energyLabel = transform.Find("EnergyProgressBar/Label").GetComponent<UILabel>();
        toughenSlider = transform.Find("ToughenProgressBar").GetComponent<UISlider>();
        toughenLabel = transform.Find("ToughenProgressBar/Label").GetComponent<UILabel>();
        energyPlusBotton = transform.Find("EnergyPlusButton").GetComponent<UIButton>();
        toughenPlusBotton = transform.Find("ToughenPlusButton").GetComponent<UIButton>();
        headButton = transform.Find("HeadButton").GetComponent<UIButton>();
        PlayerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;

        EventDelegate ed = new EventDelegate(this, "OnHeadButtonClick");
        headButton.onClick.Add(ed);
    }

    void OnDestroy()
    {
        PlayerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }

    //当主角信息发生改变，触发此方法
    void OnPlayerInfoChanged(InfoType type)
    {
        if(type == InfoType.All || type == InfoType.Name || type == InfoType.Level || type == InfoType.HeadPortrait ||
            type == InfoType.Energy || type == InfoType.Toughen)
        {
            UpdateShow();
        }
    }

    //更新显示
    void UpdateShow()
    {
        PlayerInfo info = PlayerInfo._instance;
        headSprite.spriteName = info.HeadPortrait;
        levelLabel.text = info.Level.ToString();
        nameLabel.text = info.Name.ToString();
        energySlider.value = info.Energy / 100f;
        energyLabel.text = info.Energy.ToString() + "/100";
        toughenSlider.value = info.Toughen / 50f;
        toughenLabel.text = info.Toughen.ToString() + "/50";
    }

    public void OnHeadButtonClick()
    {
        PlayerStatus._instance.show();
    }
}
