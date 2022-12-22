using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private ItemObjectInfo objectInfo;
    [SerializeField] private ItemSlotUI[] itemSlots;
    private Item lastSelectedItem;
    private SelectUIObject selectUIObject;

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
        selectUIObject = GetComponent<SelectUIObject>();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].inventoryPanel = this;
        }

        selectUIObject.selectAsFirstUI = itemSlots[0].gameObject;
        selectUIObject.SelectFirstUIElement();
    }

    private void OnEnable()
    {
        if(lastSelectedItem == null) { return; }
        UpdateUI();
    }
}
