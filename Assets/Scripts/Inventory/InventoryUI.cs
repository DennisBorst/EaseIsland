using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemSlotUI[] itemSlots;
    [SerializeField] private ItemSlotUI[] playerItemSlots;

    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    public void SelectObject(int objectCount)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemSlots[objectCount].gameObject);
    }

    public void UpdateSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots)
    {
        UpdateItemSlots(currentInvList, itemSlots.Length, itemSlots);
    }

    public void UpdatePlayerSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots)
    {
        UpdateItemSlots(currentInvList, playerItemSlots.Length, playerItemSlots);
    }

    public void CheckForButtons(InventoryManager.ItemStack itemStack)
    {
        useButton.interactable = false;
        dropButton.interactable = false;

        if (itemStack == null || itemStack.item == null) { return; }
        
        dropButton.interactable = true;
        if(itemStack.item.itemType == Item.ItemType.Food)
        {
            useButton.interactable = true;
        }
    }

    private void Start()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].Clear();
        }
    }

    private void UpdateItemSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots, ItemSlotUI[] slots)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            slots[i].Clear();
        }

        for (int i = 0; i < currentInvList.Count; i++)
        {
            if (currentInvList[i] != null)
            {
                slots[i].UpdateItem(currentInvList[i]);
            }
            else
            {
                slots[i].Clear();
            }

            if (slots.Length - 1 == i) { break; }
        }
    }
}
