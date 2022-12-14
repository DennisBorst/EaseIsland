using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftObjectInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI craftName;
    [SerializeField] private ItemHolderUI[] craftItemsNeeded;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image buildItemImg;
    [SerializeField] private Button craftButton;


    public void UpdateCraftItem(CraftableObject craftableObject)
    {
        craftName.text = craftableObject.craftableItem.ItemName;
        craftName.color = Color.red;
        craftButton.interactable = false;
        itemDescription.text = craftableObject.craftableItem.itemDescription;
        buildItemImg.sprite = craftableObject.craftableItem.inventoryImg;

        for (int i = 0; i < craftItemsNeeded.Length; i++)
        {
            craftItemsNeeded[i].ItemTextColor(Color.red);
            craftItemsNeeded[i].ObjectInvisible();
        }

        int itemAvaibleCount = 0;

        for (int i = 0; i < craftableObject.necessities.Length; i++)
        {
            craftItemsNeeded[i].ChangeItem(craftableObject.necessities[i].item, craftableObject.necessities[i].amount);
        }

        for (int i = 0; i < craftableObject.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(craftableObject.necessities[i].item, craftableObject.necessities[i].amount))
            {
                craftItemsNeeded[i].ItemTextColor(Color.green);
                itemAvaibleCount += 1;
            }
        }

        if (craftableObject.necessities.Length <= itemAvaibleCount)
        {
            craftName.color = Color.green;
            craftButton.interactable = true;
        }
    }
}
