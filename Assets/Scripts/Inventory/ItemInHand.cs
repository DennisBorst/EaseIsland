using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    public InventoryManager.ItemStack itemStack;
    [HideInInspector] public Item currentItemSelected;

    [SerializeField] private Transform itemPivot;
    [SerializeField] private ItemSlotUI[] itemSlotUI;

    private int currentItemSpot;
    private int previousItemSpot;

    public void UpdateItem()
    {
        if (itemSlotUI[previousItemSpot].itemStack == null) { RemoveItemFromHand(); }
        else if (currentItemSelected == itemSlotUI[previousItemSpot].itemStack.item) { return; }
        else
        {
            itemStack = itemSlotUI[previousItemSpot].itemStack;
            currentItemSelected = itemSlotUI[previousItemSpot].itemStack.item;
            UpdateObjectInHand();
        }
    }

    public void SwitchItemInHand(int itemSpot)
    {
        itemSlotUI[previousItemSpot].UnSelected();

        currentItemSpot += itemSpot;
        if(currentItemSpot < 0) { currentItemSpot = itemSlotUI.Length - 1; }
        else if(currentItemSpot >= itemSlotUI.Length) { currentItemSpot = 0; }

        if (itemSlotUI[currentItemSpot].itemStack != null)
        {
            itemStack = itemSlotUI[currentItemSpot].itemStack;
            currentItemSelected = itemSlotUI[currentItemSpot].itemStack.item;
        }
        else
        {
            itemStack = null;
            currentItemSelected = null;
        }

        itemSlotUI[currentItemSpot].Selected();
        previousItemSpot = currentItemSpot;

        UpdateObjectInHand();
    }

    public void RemoveItemFromHand()
    {
        itemStack = null;
        currentItemSelected = null;

        if (itemPivot.childCount != 0)
        {
            Destroy(itemPivot.GetChild(0).gameObject);
        }
    }

    private void Start()
    {
        SwitchItemInHand(0);
    }

    private void UpdateObjectInHand()
    {
        foreach (Transform child in itemPivot)
        {
            Destroy(child.gameObject);
        }


        if (currentItemSelected == null) { return; }
        GameObject item = Instantiate(currentItemSelected.prefabItem, itemPivot.position, itemPivot.rotation, itemPivot);
        item.layer = 8;
        foreach (Transform child in item.transform)
        {
            child.gameObject.layer = 8;
        }
        item.GetComponent<ItemPickup>().ColliderActive(false);
    }

    #region Singleton
    private static ItemInHand instance;
    private void Awake()
    {
        instance = this;
    }
    public static ItemInHand Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemInHand();
            }

            return instance;
        }
    }
    #endregion
}