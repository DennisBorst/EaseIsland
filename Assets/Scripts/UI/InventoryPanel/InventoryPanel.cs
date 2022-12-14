using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private ItemObjectInfo objectInfo;
    [SerializeField] private ItemSlotUI[] itemSlots;
    private Item lastSelectedItem;

    public void UpdateUI()
    {
        InventoryManager.Instance.UpdateAllUI();
        objectInfo.UpdateItemInfo(lastSelectedItem);
    }

    public void ItemSelected(Item item)
    {
        lastSelectedItem = item;
        objectInfo.UpdateItemInfo(item);
    }

    private void Start()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].inventoryPanel = this;
        }
    }

    private void OnEnable()
    {
        if(lastSelectedItem == null) { return; }
        UpdateUI();
    }
}
