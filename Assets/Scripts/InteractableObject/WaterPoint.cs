using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPoint : MonoBehaviour
{
    private Interactable interactable;
    private InteractableUI interactableUI;

    private bool bucketInHand;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (itemInHand != null)
        {
            if (itemInHand.item == ItemPickup.Item.Bucket)
            {
                return;
            }
        }
    }

    public void InRange()
    {
        if(ItemInHand.Instance.currentItemSelected == null || ItemInHand.Instance.currentItemSelected.item != ItemPickup.Item.Bucket) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }


    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }
}
