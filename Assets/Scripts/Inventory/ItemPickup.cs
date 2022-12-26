using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
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
        FullBucket,
        Pickaxe,
        FishingRod,
        CrystalB,
        CrystalR,
        CrystalG,
        Fish,
        NightFish,
        CaveFish,
        WindOrb,
        Shears
    }

    public Item item;
    public ItemType itemType;
    
    [HideInInspector] public int currentStacked;

    private Collider collider;
    private ItemUI itemUI;

    public void PickedUp()
    {
        bool canBePickedUp = InventoryManager.Instance.AddToInvWithAnim(item);

        if (canBePickedUp)
        {
            PlayerAnimation.Instance.PlayAnimCount(4);
            StartCoroutine(WaitToDestroy());
        }
    }

    public void ColliderActive(bool active)
    {
        collider.enabled = active;
    }

    public void InVision(bool inVision)
    {
        if (inVision && InventoryManager.Instance.CheckSpace(item))
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
