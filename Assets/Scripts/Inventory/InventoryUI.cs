using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemSlotUI[] itemSlots;
    [SerializeField] private ItemSlotUI[] playerItemSlots;

    [SerializeField] private GameObject moveVisObj;
    [SerializeField] private Image moveVisImg;
    [SerializeField] private TextMeshProUGUI moveVisText;


    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI[] panelText;
    [SerializeField] private Image[] panelColor;
    [SerializeField] private Color32 panelColorSelected;
    [SerializeField] private Color32 panelColorUnselected;

    private int currentPanelSpot;

    public void OpenPanel(int panelCount)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            panelText[i].color = Color.black;
            panelColor[i].color = panelColorUnselected;
        }

        panels[panelCount].SetActive(true);
        panels[panelCount].GetComponent<SelectUIObject>().SelectFirstUIElement();
        panelText[panelCount].color = Color.white;
        panelColor[panelCount].color = panelColorSelected;
    }

    public void OpenCurrentPanel()
    {
        OpenPanel(currentPanelSpot);
    }

    public void CloseCurrentPanel()
    {
        panels[currentPanelSpot].SetActive(false);
    }

    public void SwitchPanel(int panelSpot)
    {
        currentPanelSpot += panelSpot;
        if (currentPanelSpot < 0) { currentPanelSpot = panels.Length - 1; }
        else if (currentPanelSpot >= panels.Length) { currentPanelSpot = 0; }

        OpenPanel(currentPanelSpot);
    }

    public void SelectObject(int objectCount)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemSlots[objectCount].gameObject);
    }

    public void UpdatePanelPage()
    {
        panels[currentPanelSpot].GetComponent<SelectUIObject>().UpdatePanel();
    }

    public void UpdateSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots)
    {
        UpdateItemSlots(currentInvList, itemSlots.Length, itemSlots);
    }

    public void UpdatePlayerSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots)
    {
        UpdateItemSlots(currentInvList, playerItemSlots.Length, playerItemSlots);
    }

    public void MoveItemVisual(InventoryManager.ItemStack itemStack, int currentPosition)
    {
        moveVisObj.SetActive(true);
        moveVisImg.sprite = itemStack.item.inventoryImg;
        if(itemStack.amount == 1) { moveVisText.text = ""; }
        else { moveVisText.text = "" + itemStack.amount; }
        moveVisObj.gameObject.transform.position = itemSlots[currentPosition].transform.position;
    }

    public void MoveItem(int currentPosition)
    {
        itemSlots[currentPosition].EnableImg(false);
    }

    public void MoveItemDone()
    {
        moveVisObj.SetActive(false);

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].EnableImg(true);
        }
    }

    private void UpdateItemSlots(List<InventoryManager.ItemStack> currentInvList, int maxSlots, ItemSlotUI[] slots)
    {
        for (int i = 0; i < currentInvList.Count; i++)
        {
            slots[i].UpdateItem(currentInvList[i]);
            if (slots.Length - 1 == i) { break; }
        }
    }
}
