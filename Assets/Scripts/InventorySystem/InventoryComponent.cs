using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// contains all items that the player or the enemy currently has
// and drops them all on death if it receives a health component callback
public class InventoryComponent : MonoBehaviour
{
    void Awake()
    {
        if (GetComponent<HealthComponent>())
        {
            GetComponent<HealthComponent>().OnObjectDied += EmptyInventoryOntoFloor;
        }
    }
    private void EmptyInventoryOntoFloor()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            GameObject newItem = new GameObject();
            Transform thisTransform = newItem.transform;
            ItemObjectPickable pickableObject = newItem.AddComponent<ItemObjectPickable>();
            pickableObject.SetItemObject(Container[i].item, Container[i].currentAmount);
            thisTransform.position = gameObject.transform.position;
        }
    }

    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
}


public class InventorySlot
{
    public ItemObject item;
    public int currentAmount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        currentAmount = _amount;
    }
    public void AddAmount(in int value)
    {
        currentAmount += value;
        OnItemSlotAmountChanged(currentAmount);
    }
    public void EmptySlot()
    {
        currentAmount = 0;
        OnItemSlotAmountChanged(currentAmount);
    }

    public event Action<int> OnItemSlotAmountChanged;
}
