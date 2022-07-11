using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarTranscript : MonoBehaviour
{
    private UISprite headSprite;
    private UILabel nameLabel;
    private UILabel levelLabel;
    private UISlider HPSlider;
    private UILabel HPLabel;
    private UISlider energySlider;
    private UILabel energyLabel;
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
        HPSlider = transform.Find("HPProgressBar").GetComponent<UISlider>();
        HPLabel = transform.Find("HPProgressBar/Label").GetComponent<UILabel>();
        energyPlusBotton = transform.Find("EnergyPlusButton").GetComponent<UIButton>();
        toughenPlusBotton = transform.Find("ToughenPlusButton").GetComponent<UIButton>();
        headButton = transform.Find("HeadButton").GetComponent<UIButton>();
        PlayerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;
    }

    private void Start()
    {
        UpdateShow();
        TranscriptManage._instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange += this.OnPlayerHpChange;
    }

    void OnDestroy()
    {
        //PlayerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
        if (TranscriptManage._instance.player != null)
        {
            TranscriptManage._instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange -= this.OnPlayerHpChange;
        }
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
        HPSlider.value = info.HP / info.HP;
        HPLabel.text = info.HP.ToString() + "/" + info.HP.ToString();
    }

    void OnPlayerHpChange(int hp)
    {
        PlayerInfo info = PlayerInfo._instance;
        HPSlider.value = (float)hp / info.HP;
        HPLabel.text = hp + "/" + info.HP;
    }
}
