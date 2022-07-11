using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    private Inventory inventory;
    private int level;
    private int count;
    private bool isDressed = false;

    private InventoryItemDB inventoryItemDB;

    public InventoryItem()
    {

    }

    public InventoryItem(InventoryItemDB itemDB)
    {
        this.inventoryItemDB = itemDB;
        Inventory inventoryTemp;
        InventoryManager._Instance.inventoryDict.TryGetValue(itemDB.InventoryID, out inventoryTemp);
        inventory = inventoryTemp;
        Level = itemDB.Level;
        Count = itemDB.Count;
        IsDressed = itemDB.IsDressed;
    }

    public InventoryItemDB CreateInventoryItemDB()
    {
        inventoryItemDB = new InventoryItemDB();
        inventoryItemDB.Count = Count;
        inventoryItemDB.InventoryID = Inventory.ID;
        inventoryItemDB.IsDressed = IsDressed;
        inventoryItemDB.Level = Level;
        return inventoryItemDB;
    }

    public InventoryItemDB InventoryItemDB
    {
        get { return inventoryItemDB; }
    }

    public Inventory Inventory
    {
        get { return inventory; }
        set { inventory = value; }
    }
    public int Level
    {
        get { return level; }
        set 
        { 
            level = value;
            inventoryItemDB.Level = level;
        }
    }
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            inventoryItemDB.Count = Count;
        }
    }
    public bool IsDressed
    {
        get { return isDressed; }
        set
        { 
            isDressed = value;
            inventoryItemDB.IsDressed = isDressed;
        }
    }
}
