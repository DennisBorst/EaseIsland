using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTradeButton : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public TradeUIManager tradeUIManager;
    [HideInInspector] public FoodTableUI foodTable;
    public InventoryManager.ItemStack itemStack;

    public delegate void Onclick();
    public event Onclick onClick;

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
        if(tradeUIManager != null) { tradeUIManager.ItemSelected(itemStack); }
        if(foodTable != null) { foodTable.ItemSelected(itemStack); }
    }

    public void OnClick()
    {
        if (tradeUIManager != null) { tradeUIManager.MoveItem(itemStack); }
        if(foodTable != null) { foodTable.MoveItem(itemStack); }

        if(onClick != null)
        {
            onClick.Invoke();
        }
    }

    public void Interactable(bool value)
    {
        button.interactable = value;
    }

    public bool GetInteractableState()
    {
        return button.interactable;
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
