using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodTableUI : MonoBehaviour
{
    [HideInInspector] public InventoryManager.ItemStack movingItem;
    [HideInInspector] public FoodTable foodTable;

    public Slider foodSlider;
    public Slider clothSlider;
    [SerializeField] private float amountToFillClothSlider;
    [SerializeField] private float clothDownTime;

    [SerializeField] private List<ItemTradeButton> itemTradeList = new List<ItemTradeButton>();
    [SerializeField] private ItemTradeButton tradeButton;
    [SerializeField] private Image moveVisImg;
    [SerializeField] private Item emptyItem;

    [SerializeField] private List<Clothing> randomClothing = new List<Clothing>();
    [SerializeField] private List<InventoryManager.ItemStack> inventory = new List<InventoryManager.ItemStack>();

    [Serializable]
    public struct ItemDeposit
    {
        public bool anyItem;
        public bool checkItem;
        public ItemPickup.ItemType item;
        public bool checkItemType;
        public Item.ItemType itemType;
    }
    [SerializeField] private ItemDeposit itemNeeded;

    private int itemMoveLoc;
    private bool itemIsMoving;
    private InventoryManager.ItemStack itemSelected;
    private int itemSelectedIndex = 0;

    private float clothSliderCurrentAmount;
    private bool clothActive;

    public void GetInventoryItems()
    {
        inventory.Clear();
        inventory = InventoryManager.Instance.GetInventoryList();

        inventory.Add(tradeButton.itemStack);
        tradeButton.Interactable(false);

        UpdateInventoryUI();
        SelectObject(0);
    }

    public void UpdateInventory()
    {
        InventoryManager.Instance.UpdateInventoryList(inventory);
    }

    public void UpdateSlider(float amount)
    {
        foodSlider.value = amount;
    }

    public void ItemSelected(InventoryManager.ItemStack item)
    {
        itemSelected = item;
        itemSelectedIndex = inventory.IndexOf(item);

        if (itemIsMoving)
        {
            moveVisImg.gameObject.SetActive(true);
            moveVisImg.sprite = inventory[itemMoveLoc].item.inventoryImg;
            moveVisImg.gameObject.transform.position = itemTradeList[itemSelectedIndex].transform.position;
        }
    }

    public void MoveItem(InventoryManager.ItemStack itemStack)
    {
        if (!itemIsMoving && itemStack.item != emptyItem)
        {
            itemIsMoving = true;
            movingItem = itemStack;
            CheckForItem();
            itemMoveLoc = inventory.IndexOf(itemStack);
        }
        else if (itemIsMoving)
        {
            movingItem = null;
            int newLocation = inventory.IndexOf(itemSelected);
            inventory[newLocation] = inventory[itemMoveLoc];
            inventory[itemMoveLoc] = itemSelected;
            UpdateInventoryUI();
            SelectObject(newLocation);
            MoveItemDone();
            itemIsMoving = false;
        }
    }

    public void DepositItem(ItemTradeButton tradeButton)
    {
        float foodAmount = tradeButton.itemStack.item.foodAmount * tradeButton.itemStack.amount;
        FoodManager.Instance.IncreaseFood(foodAmount);

        if (clothActive) { ClothSlider(foodAmount); }

        int index = itemTradeList.IndexOf(tradeButton);
        inventory[index] = new InventoryManager.ItemStack(emptyItem);
        tradeButton.UpdateItem(inventory[index]);

        tradeButton.Interactable(false);

        if (itemIsMoving)
        {
            CheckForItem();
        }

        if (!itemTradeList[itemSelectedIndex].GetInteractableState())
        {
            SelectObject(0);
        }
        else
        {
            SelectObject(itemSelectedIndex);
        }

    }

    private void Awake()
    {
        clothSlider.maxValue = amountToFillClothSlider;
        ClothSliderActivate();

        for (int i = 0; i < itemTradeList.Count; i++)
        {
            itemTradeList[i].foodTable = this;
            itemTradeList[i].UpdateItem(new InventoryManager.ItemStack(emptyItem));
        }

        UpdateInventoryUI();
    }

    private void CheckForItem()
    {
        if (itemNeeded.checkItem && movingItem.item.item != itemNeeded.item ||
            itemNeeded.checkItemType && movingItem.item.itemType != itemNeeded.itemType ||
            movingItem.item.itemType == Item.ItemType.NotTradable)
        {
            tradeButton.Interactable(false);
        }
        else
        {
            tradeButton.Interactable(true);
        }
    }

    private void SelectObject(int objectCount)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemTradeList[objectCount].gameObject);
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            itemTradeList[i].UpdateItem(inventory[i]);
        }
    }

    private void MoveItemDone()
    {
        moveVisImg.gameObject.SetActive(false);

        if (tradeButton.itemStack.item == emptyItem)
        {
            tradeButton.Interactable(false);
        }
    }

    public void ClothSliderActivate()
    {
        if(randomClothing.Count == 0) { return; }

        clothActive = true;
        clothSlider.value = 0f;
        clothSlider.gameObject.SetActive(true);
    }


    private void ClothSlider(float amount)
    {
        clothSliderCurrentAmount += amount;
        clothSlider.value = clothSliderCurrentAmount;

        if (clothSliderCurrentAmount >= amountToFillClothSlider)
        {
            foodTable.ClothReady(RandomCloth(), clothDownTime);
            clothActive = false;
            clothSlider.gameObject.SetActive(false);
        }
    }

    private Clothing RandomCloth()
    {
        int randomClothInt = UnityEngine.Random.Range(0, randomClothing.Count);
        Clothing returnCloth = randomClothing[randomClothInt];
        randomClothing.Remove(randomClothing[randomClothInt]);
        return returnCloth;
    }
}
