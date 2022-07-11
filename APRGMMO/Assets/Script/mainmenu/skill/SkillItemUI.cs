using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItemUI : MonoBehaviour
{
    public PosType posType;
    public bool isSelect = false;
    private Skill skill;
    private UISprite sprite;
    private UIButton button;
    private UISprite Sprite
    {
        get
        {
            if(sprite == null)
            {
                sprite = GetComponent<UISprite>();
            }
            return sprite;
        }
    }
    private UIButton Button
    {
        get
        {
            if(button == null)
            {
                button = GetComponent<UIButton>();
            }
            return button;
        }
    }

    private void Start()
    {
        SkillManage._instance.OnSyncSkillComplete += this.OnSyncSkillComplete;
    }

    public void OnSyncSkillComplete()
    {
        UpdateShow();
        if (isSelect)
        {
            OnClick();
        }
    }

    void UpdateShow()
    {
        skill = SkillManage._instance.GetSkillByPosition(posType);
        Sprite.spriteName = skill.Icon;
        Button.normalSprite = skill.Icon;

    }

    void OnClick()
    {
        transform.parent.parent.SendMessage("OnSkillClick", skill);
    }

    private void OnDestroy()
    {
        if(SkillManage._instance != null)
        {
            SkillManage._instance.OnSyncSkillComplete -= this.OnSyncSkillComplete;
        }
    }
}
