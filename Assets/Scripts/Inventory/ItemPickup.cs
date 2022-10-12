using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum Item
    {
        Empty,
        Stick,
        WoodLog,
        Stone,
        Wool,
        Leaf,
        CarrotSeeds,
        Carrots,
        Apple,
        Axe,
        Bucket,
    }

    public global::Item item;
    public Item itemType;
    
    [HideInInspector] public int currentStacked;

    private Collider collider;
    private ItemUI itemUI;

    public void PickedUp()
    {
        bool canBePickedUp = InventoryManager.Instance.AddToInv(item);

        if (canBePickedUp)
        {
            StartCoroutine(WaitToDestroy());
        }
    }

    public void ColliderActive(bool active)
    {
        collider.enabled = active;
    }

    public void InVision(bool inVision)
    {
        if (inVision)
        {
            itemUI.ShowUI(true);
        }
        else
        {
            itemUI.ShowUI(false);
        }
    }

    private void Awake()
    {
        itemUI = GetComponent<ItemUI>();
        collider = GetComponent<Collider>();
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
