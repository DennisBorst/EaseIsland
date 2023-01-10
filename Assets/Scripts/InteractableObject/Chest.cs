using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    [SerializeField] private List<ChestItemButton> uiList = new List<ChestItemButton>();

    [SerializeField] private List<InventoryManager.ItemStack> inventoryItems = new List<InventoryManager.ItemStack>();
    [SerializeField] private List<InventoryManager.ItemStack> chestItems = new List<InventoryManager.ItemStack>();
    [Space]
    [SerializeField] private GameObject chestCanvas;
    [SerializeField] private Item emptyItem;
    [SerializeField] private Image moveVisImg;
    [SerializeField] private TextMeshProUGUI moveVisText;

    [SerializeField] private KeyCode closeMenuButton;
    [SerializeField] private KeyCode closeMenuSecondButton;

    private bool chestOpen;
    private Interactable interactable;
    private InteractableUI interactableUI;
    private Animator anim;

    private bool itemIsMoving;
    private InventoryManager.ItemStack itemSelected;
    private InventoryManager.ItemStack movingItem;
    private int itemMoveLoc;
    private int itemSelectedIndex = 0;


    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (chestOpen)
        {
            //Close chest
            CloseChest();
        }
        else
        {
            //OpenChest
            chestOpen = true;
            OutRange();
            CharacterMovement.Instance.FreezePlayer(true);
            anim.SetBool("OpenChest", true);
        }
    }

    public void InRange()
    {
        if (chestOpen) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    public void OpenChest()
    {
        StartCoroutine(CheckForInput());

        CharacterMovement.Instance.OpenMenu();
        chestCanvas.SetActive(true);
        GetInventoryItems();
    }

    public void ItemSelected(ChestItemButton chestItemButton)
    {
        itemSelected = chestItemButton.itemStack;
        itemSelectedIndex = uiList.IndexOf(chestItemButton);

        if (itemIsMoving)
        {
            moveVisImg.gameObject.transform.position = uiList[itemSelectedIndex].transform.position;
        }
    }

    public void MoveItem(ChestItemButton chestItemButton)
    {
        if (!itemIsMoving && chestItemButton.itemStack.item != emptyItem)
        {
            itemIsMoving = true;
            movingItem = chestItemButton.itemStack;
            itemMoveLoc = uiList.IndexOf(chestItemButton);
            StartMoveItem(itemMoveLoc, movingItem);
        }
        else if (itemIsMoving)
        {
            itemIsMoving = false;
            movingItem = null;
            //int newLocation = inventory.IndexOf(itemSelected);
            uiList[itemSelectedIndex].itemStack = uiList[itemMoveLoc].itemStack;
            uiList[itemMoveLoc].itemStack = itemSelected;
            UpdateItemList();
            SelectObject(itemSelectedIndex);
            StopMoveItem();
        }
    }

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();
        anim = GetComponent<Animator>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);

        inventoryItems.Clear();
        inventoryItems = InventoryManager.Instance.GetInventoryList();

        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].chest = this;
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            uiList[i].UpdateItem(inventoryItems[i]);
        }

        for (int i = 0; i < 12; i++)
        {
            chestItems.Add(new InventoryManager.ItemStack(emptyItem));
            uiList[i + inventoryItems.Count].UpdateItem(chestItems[i]);
        }
    }

    private void CloseChest()
    {
        chestOpen = false;
        StopAllCoroutines();
        chestCanvas.SetActive(false);
        UpdateInventory();
        CharacterMovement.Instance.CanOnlyInteract(false);
        CharacterMovement.Instance.CloseMenu();
        anim.SetBool("OpenChest", false);
    }

    private void GetInventoryItems()
    {
        inventoryItems.Clear();
        inventoryItems = InventoryManager.Instance.GetInventoryList();
        //UpdateItemList();
        UpdateInventoryUI();
        SelectObject(0);
    }

    private void UpdateItemList()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            //Update Inventory UI
            if (inventoryItems.Count > i)
            {
                inventoryItems[i] = uiList[i].itemStack;
            }
            //Update Chest UI
            else
            {
                chestItems[i - inventoryItems.Count] = uiList[i].itemStack;
            }
        }

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            //Update Inventory UI
            if (inventoryItems.Count > i)
            {
                uiList[i].UpdateItem(inventoryItems[i]);
            }
            //Update Chest UI
            else
            {
                uiList[i].UpdateItem(chestItems[i - inventoryItems.Count]);
            }
        }
    }

    private void SelectObject(int objectCount)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(uiList[objectCount].gameObject);
    }

    private void StartMoveItem(int currentPosition, InventoryManager.ItemStack itemStack)
    {
        moveVisImg.sprite = uiList[itemMoveLoc].itemStack.item.inventoryImg;
        moveVisImg.gameObject.SetActive(true);
        uiList[currentPosition].EnableImg(false);
        if (itemStack.amount == 1) { moveVisText.text = ""; }
        else { moveVisText.text = "" + itemStack.amount; }
        moveVisImg.gameObject.transform.position = uiList[itemSelectedIndex].transform.position;
    }

    private void StopMoveItem()
    {
        moveVisImg.gameObject.SetActive(false);
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].EnableImg(true);
        }
    }

    private void UpdateInventory()
    {
        InventoryManager.Instance.UpdateInventoryList(inventoryItems);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(closeMenuButton) || Input.GetKeyDown(closeMenuSecondButton))
        {
            CloseChest();
            return;
        }
    }

    private IEnumerator CheckForInput()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            CheckInput();
        }
    }
}
