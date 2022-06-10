using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : MonoBehaviour
{
    private UILabel nameLabel;
    private UISprite inventorySprite;
    private UILabel desLabel;
    private UILabel btnLabel;
    private InventoryItem it;

    private UIButton closeButton;
    private UIButton useButton;
    private UIButton useBatchingButton;
    private InventoryItemUI itUI;

    private void Awake()
    {
        nameLabel = transform.Find("Bg/NameLabel").GetComponent<UILabel>();
        inventorySprite = transform.Find("Bg/Sprite/Sprite").GetComponent<UISprite>();
        desLabel = transform.Find("Bg/Label").GetComponent<UILabel>();
        btnLabel = transform.Find("Bg/ButtonUseBatching/Label").GetComponent<UILabel>();
        closeButton = transform.Find("CloseButton").GetComponent<UIButton>();
        useButton = transform.Find("Bg/ButtonUse").GetComponent<UIButton>();
        useBatchingButton = transform.Find("Bg/ButtonUseBatching").GetComponent<UIButton>();

        EventDelegate ed1 = new EventDelegate(this, "OnClose");
        closeButton.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnUse");
        useButton.onClick.Add(ed2);

        EventDelegate ed3 = new EventDelegate(this, "OnUseBatching");
        useBatchingButton.onClick.Add(ed3);
    }

    public void Show(InventoryItem it, InventoryItemUI itUI)
    {
        this.gameObject.SetActive(true);
        this.it = it;
        this.itUI = itUI;
        nameLabel.text = it.Inventory.Name;
        inventorySprite.spriteName = it.Inventory.ICON;
        desLabel.text = it.Inventory.Des;
        btnLabel.text = "批量使用(" + it.Count.ToString() + ")";
    }

    public void OnClose()
    {
        Close();

        transform.parent.SendMessage("DisableButton");
    }

    public void Close()
    {
        clear();
        gameObject.SetActive(false);
    }

    public void OnUse()
    {
        itUI.ChangeCount(1);
        PlayerInfo._instance.InventoryUse(it, 1);
        OnClose();
    }
    public void OnUseBatching()
    {
        itUI.ChangeCount(it.Count);
        PlayerInfo._instance.InventoryUse(it, it.Count);
        OnClose();
    }

    void clear()
    {
        this.it = null;
        this.itUI = null;
    }
}
