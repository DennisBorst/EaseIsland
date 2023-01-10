using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeUIManager : MonoBehaviour
{
    [HideInInspector] public Statue statue;
    public InventoryManager.ItemStack movingItem;

    [SerializeField] private List<ItemTradeButton> itemTradeList = new List<ItemTradeButton>();
    [SerializeField] private ItemTradeButton[] tradeButtons;
    [SerializeField] private List<InventoryManager.ItemStack> inventory = new List<InventoryManager.ItemStack>();
    [SerializeField] private Item emptyItem;
    [SerializeField] private Image moveVisImg;

    [Space]
    [SerializeField] private Image summonItemImg;
    [SerializeField] private Sprite randomSprite;

    [SerializeField] private Item firstItem;
    [SerializeField] private List<Clothing> randomClothing = new List<Clothing>();
    
    private int itemMoveLoc;
    private bool itemIsMoving;
    private InventoryManager.ItemStack itemSelected;
    private int itemSelectedIndex = 0;
    private Clothing newCloth;

    [Serializable]
    public struct ItemDeposit
    {
        public bool anyItem;
        public bool checkItem;
        public ItemPickup.ItemType item;
        public bool checkItemType;
        public Item.ItemType itemType;
        public int amount;
        public int amountAlreadyFilled;
        public TextMeshProUGUI itemAmount;
    }
    [SerializeField] private ItemDeposit[] itemsNeeded;

    public void GetInventoryItems()
    {
        inventory.Clear();
        inventory = InventoryManager.Instance.GetInventoryList();

        for (int i = 0; i < tradeButtons.Length; i++)
        {
            inventory.Add(tradeButtons[i].itemStack);
            tradeButtons[i].Interactable(false);
        }

        UpdateInventoryUI();
        UpdateDepositItems();
        SelectObject(0);
    }

    public void UpdateInventory()
    {
        InventoryManager.Instance.UpdateInventoryList(inventory);
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
            UpdateDepositItems();
            SelectObject(newLocation);
            MoveItemDone();
            itemIsMoving = false;
        }
    }

    public void DepositItem(ItemTradeButton tradeButton)
    {
        for (int i = 0; i < tradeButtons.Length; i++)
        {
            if(tradeButtons[i] == tradeButton)
            {
                itemsNeeded[i].amountAlreadyFilled += tradeButton.itemStack.amount;
                if(itemsNeeded[i].amountAlreadyFilled >= itemsNeeded[i].amount) 
                { 
                    itemsNeeded[i].amountAlreadyFilled = itemsNeeded[i].amount;
                    tradeButton.Interactable(false);
                }

                int index = itemTradeList.IndexOf(tradeButton);
                inventory[index] = new InventoryManager.ItemStack(emptyItem);
                tradeButton.UpdateItem(inventory[index]);
            }
        }

        for (int i = 0; i < tradeButtons.Length; i++)
        {
            tradeButtons[i].Interactable(false);
        }

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

        UpdateDepositItems();
        CheckForNeededItems();
    }

    public void GetRandomItem()
    {
        if(firstItem != null)
        {
            InventoryManager.Instance.AddToInvWithAnim(firstItem);
            firstItem = null;
            return;
        }


        if(newCloth == null) { return; }
        ClothManager.Instance.AddCloth(newCloth);
    }

    public bool ItemAvailable()
    {
        if (randomClothing.Count == 0)
        {
            return false;
        }
        else
        {
            int randomItemInt = UnityEngine.Random.Range(0, randomClothing.Count);
            newCloth = randomClothing[randomItemInt];
            randomClothing.Remove(randomClothing[randomItemInt]);
            return true;
        }
    }

    public void ResetStatue()
    {
        if (firstItem != null) { summonItemImg.sprite = firstItem.inventoryImg; }
        else { summonItemImg.sprite = randomSprite; }

        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            itemsNeeded[i].amountAlreadyFilled = 0;
        }
    }

    private void CheckForItem()
    {
        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            if(itemsNeeded[i].amountAlreadyFilled >= itemsNeeded[i].amount ||
                itemsNeeded[i].checkItem && movingItem.item.item != itemsNeeded[i].item ||
                itemsNeeded[i].checkItemType && movingItem.item.itemType != itemsNeeded[i].itemType ||
                movingItem.item.itemType == Item.ItemType.NotTradable)
            {
                tradeButtons[i].Interactable(false);
            }
            else
            {
                tradeButtons[i].Interactable(true);
            }
        }
    }

    private void CheckForNeededItems()
    {
        int itemsCompleted = 0;

        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            if(itemsNeeded[i].amountAlreadyFilled >= itemsNeeded[i].amount)
            {
                itemsCompleted += 1;
            }
        }

        if(itemsNeeded.Length <= itemsCompleted)
        {
            statue.TradeCompleted();
        }
    }

    private void SelectObject(int objectCount)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemTradeList[objectCount].gameObject);
    }
    private void MoveItemDone()
    {
        moveVisImg.gameObject.SetActive(false);

        for (int i = 0; i < tradeButtons.Length; i++)
        {
            if(tradeButtons[i].itemStack.item == emptyItem)
            {
                tradeButtons[i].Interactable(false);
            }
        }
    }

    private void Awake()
    {
        if(firstItem != null)
        {
            summonItemImg.sprite = firstItem.inventoryImg;
        }
        else
        {
            summonItemImg.sprite = randomSprite;
        }


        for (int i = 0; i < itemTradeList.Count; i++)
        {
            itemTradeList[i].tradeUIManager = this;
            itemTradeList[i].UpdateItem(new InventoryManager.ItemStack(emptyItem));
        }

        UpdateInventoryUI();
        UpdateDepositItems();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            itemTradeList[i].UpdateItem(inventory[i]);
        }
    }

    private void UpdateDepositItems()
    {
        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            itemsNeeded[i].itemAmount.text = itemsNeeded[i].amountAlreadyFilled + "/" + itemsNeeded[i].amount;
        }
    }
}
