using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("Necessities")]
    [SerializeField] private GameObject[] SlotPanel;
    [SerializeField] private Image[] necessitieSlots;
    [SerializeField] private TextMeshProUGUI[] amountNeeded;

    [Header("CraftableItem")]
    [SerializeField] private Button craftableItemButton;
    [SerializeField] private Image craftableItemSlot;


    public void LoadInNecessities(CraftableObject craftableItem)
    {
        craftableItemSlot.sprite = craftableItem.craftableItem.inventoryImg;
        craftableItemButton.interactable = false;

        for (int i = 0; i < SlotPanel.Length; i++)
        {
            amountNeeded[i].color = Color.red;
            SlotPanel[i].SetActive(false);
        }

        for (int i = 0; i < craftableItem.necessities.Length; i++)
        {
            necessitieSlots[i].sprite = craftableItem.necessities[i].item.inventoryImg;
            amountNeeded[i].text = "" + craftableItem.necessities[i].amount;
            SlotPanel[i].SetActive(true);
        }
    }

    public void ItemTextGreen(int itemIndex)
    {
        amountNeeded[itemIndex].color = Color.green;
    }

    public void CanCraft()
    {
        craftableItemButton.interactable = true;
    }
}
