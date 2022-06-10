using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackRoleEquip : MonoBehaviour
{
    private UISprite _sprite;
    private InventoryItem it;
    private UISprite Sprite
    {
        get
        {
            if(_sprite == null)
            {
                _sprite = this.GetComponent<UISprite>();
            }
            return _sprite;
        }
    }

    public void SetId(int id)
    {
        bool isExist = InventoryManager._Instance.inventoryDict.TryGetValue(id, out Inventory inventory);
        if (isExist)
        {
            Sprite.spriteName = inventory.ICON;
        }
    }

    public void SetInventoryItem(InventoryItem it)
    {
        if (it == null) return;
        this.it = it;
        Sprite.spriteName = it.Inventory.ICON;
    }

    public void clear()
    {
        it = null;
        Sprite.spriteName = "bg_µÀ¾ß";
    }

    public void OnPress(bool isPress)
    {
        if (isPress && it != null)
        {
            object[] objectArray = new object[3];
            objectArray[0] = it;
            objectArray[1] = false;
            objectArray[2] = this;
            transform.parent.parent.SendMessage("OnInventoryClick", objectArray);
        }
    }
}
