using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmountUI : MonoBehaviour
{
    [SerializeField] private Item emptyItem;
    [SerializeField] private ItemHolderUI[] itemInInventoryUI;

    [SerializeField] private List<Item> itemTypeList = new List<Item>();

    public void UpdateItems()
    {
        for (int i = 0; i < itemTypeList.Count; i++)
        {
            int amountOfItems = InventoryManager.Instance.AmountItemInfo(itemTypeList[i]);
            itemInInventoryUI[i].ChangeAmount(amountOfItems);
        }
    }

    private void Awake()
    {
        AssignItem();
    }

    private void AssignItem()
    {
        for (int i = 0; i < itemTypeList.Count; i++)
        {
            itemInInventoryUI[i].ChangeItem(itemTypeList[i], 0);
        }
    }
}
