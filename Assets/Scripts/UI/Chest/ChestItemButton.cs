using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestItemButton : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public Chest chest;
    public InventoryManager.ItemStack itemStack;

    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemCount;

    private Button button;

    public void UpdateItem(InventoryManager.ItemStack newItem)
    {
        itemStack = newItem;
        RefreshSlot();
    }

    public void OnSelect(BaseEventData eventData)
    {
        chest.ItemSelected(this);
    }

    public void OnClick()
    {
        chest.MoveItem(this);
    }

    private void RefreshSlot()
    {
        itemImg.sprite = itemStack.item.inventoryImg;
        if (itemStack.amount > 1)
        {
            itemCount.text = "" + itemStack.amount;
        }
        else
        {
            itemCount.text = "";
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
