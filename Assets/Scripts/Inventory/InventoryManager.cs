using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public bool inventoryOpened = false;
    
    [Header("Inventory Values")]
    [SerializeField] private int maxInvSlots;

    [Header("Inventory Referenses")]
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private List<ItemStack> inInventory = new List<ItemStack>();
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingManager craftingManager;
    [SerializeField] private Transform dropPosition;

    private int itemSelectedIndex = 0;
    private ItemInHand itemInHand;
    private ItemStack itemSelected;
    private FoodManager foodManager;

    [Serializable]
    public class ItemStack
    {
        public bool isFull => amount >= item.maxStack;

        public Item item;
        public int amount;

        public ItemStack(Item item)
        {
            this.item = item;
            amount = 1;
        }
    }

    public void OpenInventory()
    {
        inventoryOpened = true;
        craftingManager.CheckItems();
        inventoryCanvas.SetActive(true);
        inventoryUI.SelectObject(0);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void CloseInventory()
    {
        inventoryOpened = false;
        inventoryCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool AddToInv(Item item)
    {
        ItemStack itemStack = GetItemStack(item);

        if (itemStack == null || itemStack.isFull)
        {
            if (!SlotsAvailable()) { return false; }
            inInventory.Add(new ItemStack(item));
        }
        else
        {
            itemStack.amount += 1;
        }

        if(item.itemType != Item.ItemType.Tools)
        {
            PlayerAnimation.Instance.PlayAnimCount(4);
        }
        //StartCoroutine(AddItemInHandAfterTime(itemStack));
        UpdateAllUI();
        return true;
    }


    public void RemoveFromInv(Item item)
    {
        ItemStack itemStack = GetItemStack(item);
        itemStack.amount -= 1;

        if(itemStack.amount <= 0)
        {
            inInventory.Remove(itemStack);
            itemSelected = null;
        }
        UpdateAllUI();
    }

    public void RemoveItemStackFromInv(ItemStack itemStack)
    {
        itemStack.amount -= 1;

        if (itemStack.amount <= 0)
        {
            inInventory.Remove(itemStack);
            itemSelected = null;
        }
        UpdateAllUI();
    }

    public void UseFoodItem(bool fromHand = false)
    {
        if (fromHand)
        {
            PlayerAnimation.Instance.PlayAnimCount(2);
            StartCoroutine(DeleteFoodAfterTime(itemInHand.itemStack, itemInHand.itemStack.item.foodAmount));
        }
        else
        {
            foodManager.IncreaseFood(itemSelected.item.foodAmount);
            RemoveItemStackFromInv(itemSelected);
        }
    }

    public void DropItemFromInv(bool fromHand = false)
    {
        if (fromHand && itemInHand.itemStack.item != null) { itemSelected = itemInHand.itemStack; }
        if (itemSelected.item == null) { return; }

        Instantiate(itemSelected.item.prefabItem, dropPosition.position, dropPosition.rotation);
        RemoveItemStackFromInv(itemSelected);
    }

    public void ItemSelected(ItemStack item)
    {
        itemSelected = item;
        itemSelectedIndex = inInventory.IndexOf(itemSelected);
        inventoryUI.CheckForButtons(itemSelected);
    }

    public void ItemCrafted(CraftableObject craftableItem)
    {
        for (int i = 0; i < craftableItem.necessities.Length; i++)
        {
            for (int j = 0; j < craftableItem.necessities[i].amount; j++)
            {
                RemoveFromInv(craftableItem.necessities[i].item);
            }
        }

        UpdateAllUI();
    }

    public void BuildingCompleted(Building building)
    {
        for (int i = 0; i < building.necessities.Length; i++)
        {
            for (int j = 0; j < building.necessities[i].amount; j++)
            {
                RemoveFromInv(building.necessities[i].item);
            }
        }

        UpdateAllUI();
    }

    public bool AmountOfItem(Item item, int neededAmount)
    {
        int amount = inInventory.Where(x => x.item == item).Sum(x => x.amount);
        if (amount >= neededAmount)
        {
            return true;
        }
        return false;


        //List<ItemStack> itemStacks = new List<ItemStack>();
        //int amountItems = 0;
        //for (int i = 0; i < inInventory.Count; i++)
        //{
        //    if(inInventory[i].item == item)
        //    {
        //        itemStacks.Add(inInventory[i]);
        //    }
        //}

        //for (int i = 0; i < itemStacks.Count; i++)
        //{
        //    amountItems += itemStacks[i].amount;
        //}
    }

    private void UpdateAllUI()
    {
        //inventoryUI.SelectObject(itemSelectedIndex);
        inventoryUI.UpdateSlots(inInventory, maxInvSlots);
        inventoryUI.UpdatePlayerSlots(inInventory, maxInvSlots);
        inventoryUI.CheckForButtons(itemSelected);
        itemInHand.UpdateItem();
    }

    private IEnumerator DeleteFoodAfterTime(ItemStack itemStack, float foodAmount)
    {
        yield return new WaitForSeconds(1f);
        foodManager.IncreaseFood(foodAmount);
        RemoveItemStackFromInv(itemStack);
    }

    private ItemStack GetItemStack(Item item)
    {
        ItemStack itemStack = inInventory.FindAll(x => x.item == item).OrderBy(x => x.amount).FirstOrDefault();
        return itemStack;
    }

    private bool SlotsAvailable()
    {
        return inInventory.Count < maxInvSlots;
    }

    #region Singleton
    private static InventoryManager instance;
    private void Awake()
    {
        instance = this;
        itemInHand = GetComponent<ItemInHand>();
        foodManager = GetComponent<FoodManager>();
    }
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InventoryManager();
            }

            return instance;
        }
    }
    #endregion
}