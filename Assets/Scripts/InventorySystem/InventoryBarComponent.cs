using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// visualization above the player's head for what they currently have in their inventory, and what number.
// should be one for uranium and one for carbon
// requires watchedItem, inventoryIconImage, and inventoryQuantityText components being set in the inspector
public class InventoryBarComponent : MonoBehaviour
{
    [SerializeField]
    private ItemObject watchedItem;

    [SerializeField]
    private Image inventoryIconImage;

    [SerializeField]
    private Text inventoryQuantityText;
    
    private void Awake()
    {
        List<InventorySlot> inventorySlots = GetComponentInParent<InventoryComponent>().Container;
        for (int i= 0; i < inventorySlots.Count; i++)
        {
            inventoryIconImage.sprite = watchedItem.UIImage;
            if (inventorySlots[i].item == watchedItem)
            {
                inventorySlots[i].OnItemSlotAmountChanged += ItemAmountChanged;
                ItemAmountChanged(inventorySlots[i].currentAmount);
                return;
            }
        }
        GetComponentInParent<InventoryComponent>().AddItem(watchedItem, 0);
    }

    private void ItemAmountChanged(int newItemAmount)
    {
        inventoryQuantityText.text = newItemAmount.ToString();
    }
}
