using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [HideInInspector] public InventoryManager.ItemStack itemStack;

    [SerializeField] private Image backGroundImg;
    [SerializeField] private Color32 backGroundColorSelected;
    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemCount;

    private Color32 backGroundColor;
    private Button interactableButton;

    public void UpdateItem(InventoryManager.ItemStack newItem)
    {
        itemStack = newItem;
        RefreshSlot();
    }

    public void Selected()
    {
        backGroundImg.color = backGroundColorSelected;
    }

    public void UnSelected()
    {
        backGroundImg.color = backGroundColor;
    }

    public void Clear()
    {
        itemImg.gameObject.SetActive(false);
        interactableButton.interactable = false;
        itemStack = null;
        itemCount.text = "";
    }

    public void SelectThisItem()
    {
        if(itemStack == null) { return; }
        InventoryManager.Instance.ItemSelected(itemStack);
    }

    private void Awake()
    {
        backGroundColor = backGroundImg.color;
        interactableButton = GetComponent<Button>();
    }

    private void RefreshSlot()
    {
        itemImg.gameObject.SetActive(true);
        interactableButton.interactable = true;
        itemImg.sprite = itemStack.item.inventoryImg;
        if(itemStack.amount > 1)
        {
            itemCount.text = "" + itemStack.amount;
        }
        else
        {
            itemCount.text = "";
        }
    }
}
