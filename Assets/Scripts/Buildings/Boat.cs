using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private Interactable interactable;
    private InteractableUI interactableUI;


    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        GameManger.Instance.Fade(0);
    }

    public void InRange()
    {
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
