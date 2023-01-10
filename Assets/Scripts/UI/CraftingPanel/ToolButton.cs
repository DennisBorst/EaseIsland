using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ToolButton : MonoBehaviour, ISelectHandler
{
    public CraftingPanel craftingPanel;
    public CraftableObject craftObject;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI craftName;
    [SerializeField] private Color32 unableToCraft;
    [SerializeField] private Color32 ableToCraft;

    public void OnSelect(BaseEventData eventData)
    {
        craftingPanel.ToolSelected(craftObject, this.gameObject);
    }

    public void UpdateUI()
    {
        craftName.text = craftObject.craftableItem.ItemName;
        UpdateCraftItem(craftObject);
    }

    public void UpdateCraftItem(CraftableObject craftableObject)
    {
        craftName.color = unableToCraft;
        int itemAvaibleCount = 0;

        for (int i = 0; i < craftableObject.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(craftableObject.necessities[i].item, craftableObject.necessities[i].amount))
            {
                itemAvaibleCount += 1;
            }
        }

        if (craftableObject.necessities.Length <= itemAvaibleCount)
        {
            craftName.color = ableToCraft;
        }
    }
}
