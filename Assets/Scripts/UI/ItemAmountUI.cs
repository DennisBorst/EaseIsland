using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmountUI : MonoBehaviour
{
    [SerializeField] private ItemHolderUI[] itemInInventoryUI;
    
    public void UpdateItems()
    {
        for (int i = 0; i < itemInInventoryUI.Length; i++)
        {
            itemInInventoryUI[i].ObjectInvisible();
        }

        List<Item> currentItems = new List<Item>();
        currentItems = InventoryManager.Instance.GetItemInfo();

        for (int i = 0; i < currentItems.Count; i++)
        {
            int amountOfItems = InventoryManager.Instance.AmountItemInfo(currentItems[i]);
            itemInInventoryUI[i].ChangeItem(currentItems[i], amountOfItems);
        }
    }

    public void UpdateInventory()
    {

    }
}
