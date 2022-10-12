using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] public UnityEvent<Item, Vector3> doAction = new UnityEvent<Item, Vector3>();
    [SerializeField] public UnityEvent inRange;
    [SerializeField] public UnityEvent outRange;

    public void Interact(InventoryManager.ItemStack itemInHand, Vector3 playerPos)
    {
        if(itemInHand == null)
        {
            doAction.Invoke(null, playerPos);
        }
        else
        {
            doAction.Invoke(itemInHand.item, playerPos);
        }
    }

    public void InRange()
    {
        inRange.Invoke();
    }

    public void OutRange()
    {
        outRange.Invoke();
    }
}
