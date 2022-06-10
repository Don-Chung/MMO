using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI _instance;
    public List<InventoryItemUI> itemUIList = new List<InventoryItemUI>(); //������Ʒ�ĸ���
    private UIButton clearupButton;
    private UILabel inventoryLabel;

    private int count = 0; //���ٸ�������Ʒ

    private void Awake()
    {
        _instance = this;
        InventoryManager._Instance.OnInventoryChange += this.OnInventoryChange;
        clearupButton = transform.Find("ButtonClearup").GetComponent<UIButton>();
        inventoryLabel = transform.Find("InventoryLabel").GetComponent<UILabel>();

        EventDelegate ed = new EventDelegate(this, "OnClearup");
        clearupButton.onClick.Add(ed);
    }

    private void OnDestroy()
    {
        InventoryManager._Instance.OnInventoryChange -= this.OnInventoryChange;
    }

    void OnInventoryChange()
    {
        UpdateShow();
    }

    void UpdateShow()
    {
        int tmp = 0;
        for (int i = 0; i < InventoryManager._Instance.inventoryitemList.Count; i++)
        {   
            InventoryItem it = InventoryManager._Instance.inventoryitemList[i];
            if(it.IsDressed == false)
            {
                itemUIList[tmp].SetInventoryItem(it);
                ++tmp;
            }
        }
        count = tmp;
        for(int i = tmp; i < itemUIList.Count; i++)
        {
            itemUIList[i].clear();
        }
        inventoryLabel.text = count.ToString() + "/32";
    }

    public void UpdateCount()
    {
        count = 0;
        foreach (InventoryItemUI itUI in itemUIList)
        {
            if (itUI.it != null)
            {
                count++;
            }
        }
        inventoryLabel.text = count.ToString() + "/32";
    }

    public void AddInventoryItem(InventoryItem it)
    {
        foreach(InventoryItemUI itUI in itemUIList)
        {
            if(itUI.it == null)
            {
                itUI.SetInventoryItem(it);
                count++;
                break;
            } 
        }
        inventoryLabel.text = count.ToString() + "/32";
    }

    //�������ťʱ����
    void OnClearup()
    {
        UpdateShow();
    }
}
