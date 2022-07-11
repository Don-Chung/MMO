using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager _Instance;
    public TextAsset listinfo;
    public Dictionary<int, Inventory> inventoryDict = new Dictionary<int, Inventory>();
    //public Dictionary<int, InventoryItem> inventoryitemDict = new Dictionary<int, InventoryItem>();
    public List<InventoryItem> inventoryitemList = new List<InventoryItem>();

    public delegate void OnInventoryChangeEvent();
    public event OnInventoryChangeEvent OnInventoryChange;

    private InventoryItemDBController inventoryItemDBController;

    private void Awake()
    {
        _Instance = this;
        inventoryItemDBController = this.GetComponent<InventoryItemDBController>();
        inventoryItemDBController.OnGetInventoryItemDBList += this.OnGetInventoryItemDBList;
        inventoryItemDBController.OnAddInventoryItemDB += this.OnAddInventoryItemDB;
        ReadInventoryInfo();
    }

    private void Start()
    {
        ReadInventoryItemInfo();
    }

    private void Update()
    {
        PickUp();
    }

    void ReadInventoryInfo()
    {
        string str = listinfo.ToString();
        string[] itemStrArray = str.Split('\n');
        for(int i = 1; i < itemStrArray.Length; ++i)
        {
            //ID ���� ͼ�� ���ͣ�Equip��Drug�� װ������(Helm,Cloth,Weapon,Shoes,Necklace,Bracelet,Ring,Wing) �ۼ� �Ǽ� Ʒ�� �˺� ���� ս���� �������� ����ֵ ����
            string[] proArray = itemStrArray[i].Split('|');
            Inventory inventory = new Inventory();
            inventory.ID = int.Parse(proArray[0]);
            inventory.Name = proArray[1];
            inventory.ICON = proArray[2];
            switch (proArray[3])
            {
                case "Equip":
                    inventory.InverntoryTYPE = InverntoryType.Equip; 
                    break;
                case "Drug":
                    inventory.InverntoryTYPE = InverntoryType.Drug;
                    break;
                case "Box":
                    inventory.InverntoryTYPE = InverntoryType.Box;
                    break;
            }
            if (inventory.InverntoryTYPE == InverntoryType.Equip)
            {
                switch (proArray[4])
                {
                    case "Helm":
                        inventory.EquipTYPE = EquipType.Helm;
                        break;
                    case "Cloth":
                        inventory.EquipTYPE = EquipType.Cloth;
                        break;
                    case "Weapon":
                        inventory.EquipTYPE = EquipType.Weapon;
                        break;
                    case "Shoes":
                        inventory.EquipTYPE = EquipType.Shoes;
                        break;
                    case "Necklace":
                        inventory.EquipTYPE = EquipType.Necklace;
                        break;
                    case "Bracelet":
                        inventory.EquipTYPE = EquipType.Bracelet;
                        break;
                    case "Ring":
                        inventory.EquipTYPE = EquipType.Ring;
                        break;
                    case "Wing":
                        inventory.EquipTYPE = EquipType.Wing;
                        break;
                }
            }
            //�ۼ� �Ǽ� Ʒ�� �˺� ���� ս���� �������� ����ֵ ����
            inventory.Price = int.Parse(proArray[5]);
            if(inventory.InverntoryTYPE == InverntoryType.Equip)
            {
                inventory.StarLevel = int.Parse(proArray[6]);
                inventory.Quality = int.Parse(proArray[7]);
                inventory.Damage = int.Parse(proArray[8]);
                inventory.HP = int.Parse(proArray[9]);
                inventory.Power = int.Parse(proArray[10]);
            }
            if(inventory.InverntoryTYPE == InverntoryType.Drug)
            {
                inventory.ApplyValue = int.Parse(proArray[12]);
            }
            inventory.Des = proArray[13];
            inventoryDict.Add(inventory.ID, inventory);
        }
    }

    //��ɽ�ɫ������Ϣ��ʼ�������ӵ�е���Ʒ
    void ReadInventoryItemInfo()
    {
        //TODO ��Ҫ���ӷ�������ý�ɫӵ�е���Ʒ��Ϣ


        //������ɣ�test
        //for(int i = 0; i < 20; ++i)
        //{
        //    int id = Random.Range(1001, 1020);
        //    inventoryDict.TryGetValue(id, out Inventory invent);
        //    if(invent.InverntoryTYPE == InverntoryType.Equip)
        //    {
        //        InventoryItem it = new InventoryItem();
        //        it.Inventory = invent;
        //        it.Level = Random.Range(1, 20);
        //        it.Count = 1;
        //        inventoryitemList.Add(it);
        //    }
        //    else
        //    {
        //        //���жϱ������Ƿ����
        //        InventoryItem it = null;
        //        bool isExist = false;
        //        foreach(InventoryItem temp in inventoryitemList)
        //        {
        //            if(temp.Inventory.ID == id)
        //            {
        //                isExist = true;
        //                it = temp;
        //                break;
        //            }
        //        }
        //        if (isExist)
        //        {
        //            it.Count++;
        //        }
        //        else
        //        {
        //            it = new InventoryItem();
        //            it.Inventory = invent;
        //            it.Count = 1;
        //            inventoryitemList.Add(it);
        //        }
        //    }
        //}
        inventoryItemDBController.GetInventoryItemDB();



    }

    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int id = Random.Range(1001, 1020);
            inventoryDict.TryGetValue(id, out Inventory invent);
            if(invent.InverntoryTYPE == InverntoryType.Equip)
            {
                //InventoryItem it = new InventoryItem();
                //it.Inventory = invent;
                //it.Level = Random.Range(1, 20);
                //it.Count = 1;
                //inventoryitemList.Add(it);
                //InventoryItemDB itemDB = it.CreateInventoryItemDB();
                InventoryItemDB itemDB = new InventoryItemDB();
                itemDB.InventoryID = id;
                itemDB.Count = 1;
                itemDB.IsDressed = false;
                itemDB.Level = Random.Range(1, 10);
                inventoryItemDBController.AddInventoryItemDB(itemDB);
            }
            else
            {
                //���жϱ������Ƿ����
                InventoryItem it = null;
                bool isExist = false;
                foreach (InventoryItem temp in inventoryitemList)
                {
                    if (temp.Inventory.ID == id)
                    {
                        isExist = true;
                        it = temp;
                        break;
                    }
                }
                if (isExist)
                {
                    it.Count++;
                    //ͬ��inventoryitemdb ����
                    inventoryItemDBController.UpdateInventoryItemDB(it.InventoryItemDB);
                }
                else
                {
                    InventoryItemDB itemDB = new InventoryItemDB();
                    itemDB.InventoryID = id;
                    itemDB.Count = 1;
                    itemDB.IsDressed = false;
                    itemDB.Level = Random.Range(1, 10);
                    inventoryItemDBController.AddInventoryItemDB(itemDB);
                }
            }
        }
    }

    public void OnAddInventoryItemDB(InventoryItemDB itemDB) 
    {
        InventoryItem it = new InventoryItem(itemDB);
        inventoryitemList.Add(it);

        OnInventoryChange();
    }

    public void OnGetInventoryItemDBList(List<InventoryItemDB> list)
    {
        foreach(var itemDB in list)
        {
            InventoryItem it = new InventoryItem(itemDB);
            inventoryitemList.Add(it);
        }
        OnInventoryChange();
    }

    public void RemoveInventoryItem(InventoryItem it)
    {
        this.inventoryitemList.Remove(it);
    }

    public void UpgradeEquip(InventoryItem it)
    {
        inventoryItemDBController.UpgradeEquip(it.InventoryItemDB);
    }

    private void OnDestroy()
    {
        if(inventoryItemDBController != null)
        {
            inventoryItemDBController.OnGetInventoryItemDBList -= this.OnGetInventoryItemDBList;
        }
    }
}
