using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    private UISprite sprite;
    private UILabel label;
    public InventoryItem it;

    private UISprite Sprite
    {
        get
        {
            if(sprite == null)
            {
                sprite = transform.Find("Sprite").GetComponent<UISprite>();
            }
            return sprite;
        }
    }
    private UILabel Label
    {
        get
        {
            if(label == null)
            {
                label = transform.Find("Label").GetComponent<UILabel>();
            }
            return label;
        }
    }
    public void SetInventoryItem(InventoryItem it)
    {
        this.it = it;
        Sprite.spriteName = it.Inventory.ICON;
        if(it.Count <= 1)
        {
            Label.text = "";
        }
        else
        {
            Label.text = it.Count.ToString();
        }
    }
    public void clear()
    {
        it = null;
        Label.text = "";
        Sprite.spriteName = "bg_µÀ¾ß";
    }
    public void OnClick()
    {
        if (it != null)
        {
            object[] objectArray = new object[3];
            objectArray[0] = it;
            objectArray[1] = true;
            objectArray[2] = this;
            transform.parent.parent.parent.SendMessage("OnInventoryClick", objectArray);
        }
    }

    public void ChangeCount(int count)
    {
        if(it.Count - count <= 0)
        {
            clear();
        }else if(it.Count - count == 1)
        {
            Label.text = "";
        }
        else
        {
            Label.text = (it.Count - count).ToString();
        }
    }
}
