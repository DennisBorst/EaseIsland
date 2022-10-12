using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private CraftableObject[] craftableItems;

    private int necessitieChecker;
    private CraftingUI craftingUI;
    private CraftableObject selectedItem;

    public void CheckItems(CraftableObject craftableItem = null)
    {
        necessitieChecker = 0;
        if(craftableItem == null)
        {
            if(selectedItem == null) { selectedItem = craftableItems[0]; }
        }
        else { selectedItem = craftableItem; }

        craftingUI.LoadInNecessities(selectedItem);

        for (int i = 0; i < selectedItem.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(selectedItem.necessities[i].item, selectedItem.necessities[i].amount))
            {
                necessitieChecker += 1;
                craftingUI.ItemTextGreen(i);
            }
        }

        if(selectedItem.necessities.Length <= necessitieChecker)
        {
            craftingUI.CanCraft();
        }
    }

    public void CraftItem()
    {
        InventoryManager.Instance.ItemCrafted(selectedItem);
        InventoryManager.Instance.AddToInv(selectedItem.craftableItem);
        craftingUI.LoadInNecessities(selectedItem);
    }

    private void Awake()
    {
        craftingUI = GetComponent<CraftingUI>();
    }
}
