using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemSlotUI[] itemSlots;
    [SerializeField] private ItemSlotUI[] playerItemSlots;

    [SerializeField] private Image moveVisImg;

    [SerializeField] private GameObject[] panels;
    [SerializeField] private TextMeshProUGUI[] panelText;

    private int currentPanelSpot;

    public void OpenPanel(int panelCount)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            panelText[i].color = Color.black;
        }

        panels[panelCount].SetActive(true);
        panels[panelCount].GetComponent<SelectUIObject>().SelectFirstUIElemnt();
        panelText[panelCount].color = Color.white;
    }

    public void OpenCurrentPanel()
    {
        OpenPanel(currentPanelSpot);
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
        moveVisImg.gameObject.SetActive(true);
        moveVisImg.sprite = itemStack.item.inventoryImg;
        moveVisImg.gameObject.transform.position = itemSlots[currentPosition].transform.position;
    }

    public void MoveItemDone()
    {
        moveVisImg.gameObject.SetActive(false);
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
