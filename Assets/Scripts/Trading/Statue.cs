using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField] private GameObject tradeUIManagerCanvas;
    [SerializeField] private TradeUIManager tradeUIManager;
    [SerializeField] private GameObject itemAnim;

    private bool menuOpened;
    private Interactable interactable;
    private InteractableUI interactableUI;
    private Animator anim;

    private bool itemPrepare;
    private bool itemReady;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (itemPrepare) { return; }

        if (itemReady)
        {
            itemReady = false;
            itemPrepare = true;
            OutRange();
            tradeUIManager.GetRandomItem(itemAnim);
            anim.SetTrigger("ItemPickedUp");
            return;
        }

        if (menuOpened)
        {
            CloseMenu();
        }
        else
        {
            menuOpened = true;
            OutRange();
            CharacterMovement.Instance.OpenMenu();
            CharacterMovement.Instance.CanOnlyInteract(true);
            tradeUIManagerCanvas.SetActive(true);
            tradeUIManager.GetInventoryItems();
        }
    }

    public void InRange()
    {
        if (menuOpened || itemPrepare) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    public void TradeCompleted()
    {
        CloseMenu();
        itemPrepare = true;
        anim.SetTrigger("ItemReady");
    }

    public void ItemReady()
    {
        itemPrepare = false;
        itemReady = true;
    }

    public void MenuReady()
    {
        if (tradeUIManager.ItemAvailable())
        {
            itemPrepare = false;
            tradeUIManager.ResetStatue();
        }
        else
        {
            anim.SetTrigger("StatueEmpty");
        }
    }

    public void CloseMenu()
    {
        tradeUIManagerCanvas.SetActive(false);
        tradeUIManager.UpdateInventory();
        CharacterMovement.Instance.CanOnlyInteract(false);
        CharacterMovement.Instance.CloseMenu();
        menuOpened = false;
    }

    private void Awake()
    {
        tradeUIManager.statue = this;
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();
        anim = GetComponent<Animator>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }
}
