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
    [SerializeField] private GameObject buildTextPopUp;
    [SerializeField] private List<ItemStack> inInventory = new List<ItemStack>();
    [SerializeField] private InventoryUI inventoryUI;
    //[SerializeField] private CraftingManager craftingManager;
    [SerializeField] private Transform dropPosition;
    [SerializeField] private Item emptyItem;
    [SerializeField] private GameObject itemAnimObject;
    [SerializeField] private Transform backPackLoc;


    private int itemSelectedIndex = 0;
    private ItemInHand itemInHand;
    private ItemStack itemSelected;
    private int itemMoveLoc;
    private bool itemMoving;
    private FoodManager foodManager;


    [Serializable]
    public class ItemStack
    {
        public bool isFull => amount >= item.maxStack;

        public Item item;
        public int amount;

        public ItemStack(Item item, int amount = 1)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public void OpenInventory()
    {
        inventoryOpened = true;
        UpdateAllUI();
        inventoryUI.OpenCurrentPanel();
        buildTextPopUp.SetActive(false);
        inventoryCanvas.SetActive(true);
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
    }

    public void CloseInventory()
    {
        inventoryOpened = false;
        inventoryUI.CloseCurrentPanel();
        inventoryCanvas.SetActive(false);
        inventoryUI.MoveItemDone();
        itemMoving = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void SwitchPanel(int panelSpot)
    {
        inventoryUI.SwitchPanel(panelSpot);
    }


    #region Inventory
    public List<ItemStack> GetInventoryList()
    {
        List<ItemStack> itemList = new List<ItemStack>();

        for (int i = 0; i < inInventory.Count; i++)
        {
            itemList.Add(new ItemStack(inInventory[i].item, inInventory[i].amount));
        }
        return itemList;
    }

    public void UpdateInventoryList(List<ItemStack> newList)
    {
        for (int i = 0; i < inInventory.Count; i++)
        {
            inInventory[i] = newList[i];
        }

        UpdateAllUI();
    }
    
    public bool AddToInv(Item item)
    {
        ItemStack itemStack = GetItemStack(item);

        if (itemStack == null || itemStack.isFull)
        {
            if (!SlotsAvailable()) { return false; }
            int emptyObjLoc = FindFirstEmptyObject();
            inInventory[emptyObjLoc] = new ItemStack(item);
        }
        else
        {
            itemStack.amount += 1;
        }

        UpdateAllUI();
        return true;
    }

    public bool AddToInvWithAnim(Item item, int itemCount = 1)
    {
        ItemStack itemStack = GetItemStack(item);

        if (itemStack == null || itemStack.isFull)
        {
            if (!SlotsAvailable()) { return false; }
        }
        Instantiate(itemAnimObject, transform.position, new Quaternion(0,0,0,0), backPackLoc).GetComponent<GainItem>().ChangeItem(item, item.color, itemCount);
        return true;
    }

    public bool CheckSpace(Item item)
    {
        ItemStack itemStack = GetItemStack(item);

        if (itemStack == null || itemStack.isFull)
        {
            if (!SlotsAvailable()) { return false; }
        }
        return true;
    }

    public void RemoveFromInv(Item item)
    {
        ItemStack itemStack = GetItemStack(item);
        itemStack.amount -= 1;

        if(itemStack.amount <= 0)
        {
            int itemLoc = inInventory.IndexOf(itemStack);
            inInventory[itemLoc].item = emptyItem;
            itemSelected = null;
        }
        UpdateAllUI();
    }

    public void RemoveItemStackFromInv(ItemStack itemStack)
    {
        itemStack.amount -= 1;

        if (itemStack.amount <= 0)
        {
            int itemLoc = inInventory.IndexOf(itemStack);
            inInventory[itemLoc].item = emptyItem;
            itemSelected = null;
        }
        UpdateAllUI();
    }

    public void UseFoodItem(bool fromHand = false)
    {
        if (fromHand)
        {
            //PlayerAnimation.Instance.PlayAnimCount(2);
            StartCoroutine(DeleteFoodAfterTime(itemInHand.itemStack, itemInHand.itemStack.item.foodAmount));
        }
        else
        {
            if(itemSelected.item.itemType != Item.ItemType.Food) { return; }
            foodManager.IncreaseFood(itemSelected.item.foodAmount);
            RemoveItemStackFromInv(itemSelected);
        }
    }

    public void DropItemFromInv(bool fromHand = false)
    {
        if (fromHand) { itemSelected = itemInHand.itemStack; }
        if (itemSelected.item == emptyItem || itemMoving) { return; }

        int newLocation = inInventory.IndexOf(itemSelected);
        Instantiate(itemSelected.item.prefabItem, dropPosition.position, dropPosition.rotation);
        RemoveItemStackFromInv(itemSelected);
        inventoryUI.SelectObject(newLocation);
    }

    public void UseItemFromHand()
    {
        if (itemInHand.itemStack.item != null) { itemSelected = itemInHand.itemStack; }
        if (itemSelected.item == null) { return; }
        RemoveItemStackFromInv(itemSelected);
    }

    public void ItemSelected(ItemStack item)
    {
        itemSelected = item;
        itemSelectedIndex = inInventory.IndexOf(itemSelected);

        if (itemMoving)
        {
            inventoryUI.MoveItemVisual(inInventory[itemMoveLoc], itemSelectedIndex);
        }
    }

    public void MoveItem(ItemStack itemStack)
    {
        if (!itemMoving && itemStack.item != emptyItem)
        {
            itemMoving = true;
            itemMoveLoc = inInventory.IndexOf(itemStack);
        }
        else if(itemMoving)
        {
            int newLocation = inInventory.IndexOf(itemSelected);
            inInventory[newLocation] = inInventory[itemMoveLoc];
            inInventory[itemMoveLoc] = itemSelected;
            UpdateAllUI();
            inventoryUI.SelectObject(newLocation);
            inventoryUI.MoveItemDone();
            itemMoving = false;
        }
    }
    #endregion

    #region CraftPanel
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
    #endregion

    #region BuildPanel
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

    public void BuildInfoSelected(Building buildObject)
    {

    }

    #endregion

    #region General
    public bool AmountOfItem(Item item, int neededAmount)
    {
        int amount = inInventory.Where(x => x.item == item).Sum(x => x.amount);
        if (amount >= neededAmount)
        {
            return true;
        }
        return false;
    }

    public List<Item> GetItemInfo()
    {
        List<Item> itemInfo = new List<Item>();

        for (int i = 0; i < inInventory.Count; i++)
        {
            if(inInventory[i].item != emptyItem && !itemInfo.Contains(inInventory[i].item))
            {
                itemInfo.Add(inInventory[i].item);
            }
        }

        return itemInfo;
    }

    public int AmountItemInfo(Item item)
    {
        int amount = inInventory.Where(x => x.item == item).Sum(x => x.amount);
        return amount;
    }

    private void Start()
    {
        inventoryUI.UpdateSlots(inInventory, maxInvSlots);
        inventoryUI.UpdatePlayerSlots(inInventory, maxInvSlots);
    }

    public void UpdateAllUI()
    {
        inventoryUI.UpdateSlots(inInventory, maxInvSlots);
        inventoryUI.UpdatePlayerSlots(inInventory, maxInvSlots);
        inventoryUI.UpdatePanelPage();
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
        int inventoryCount = 0;

        for (int i = 0; i < inInventory.Count; i++)
        {
            if (inInventory[i].item != emptyItem) { inventoryCount += 1; }
        }

        return inventoryCount < maxInvSlots;
    }

    private int FindFirstEmptyObject()
    {
        int obj = 0;

        for (int i = 0; i < inInventory.Count; i++)
        {
            if(inInventory[i].item == emptyItem)
            {
                obj = i;
                break;
            }
        }

        return obj;
    }

    private void CreateList()
    {
        for (int i = 0; i < maxInvSlots; i++)
        {
            inInventory.Add(new ItemStack(emptyItem));
        }

        UpdateAllUI();
    }
    #endregion

    #region Singleton
    private static InventoryManager instance;
    private void Awake()
    {
        instance = this;
        itemInHand = GetComponent<ItemInHand>();
        foodManager = GetComponent<FoodManager>();

        CreateList();
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