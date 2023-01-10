using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainItem : MonoBehaviour
{
    public Item itemGained;
    [SerializeField] private Transform itemLoc;
    [SerializeField] private ParticleSystem particle;
    private Animator anim;

    private int amountOfItems;
    private int animCount = 0;

    public void ChangeItem(Item item, Color color, int itemCount = 1)
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
        this.transform.position += (Vector3.up * 1.5f);

        float animSpeed = 0.8f * itemCount;
        if(itemCount > 1) { anim.SetFloat("AnimSpeed", animSpeed); }

        amountOfItems = itemCount;
        animCount += 1;

        GameObject itemObj = Instantiate(item.prefabItem, itemLoc.position, itemLoc.rotation, itemLoc);
        itemGained = item;
        itemObj.layer = 8;
        foreach (Transform child in itemObj.transform)
        {
            child.gameObject.layer = 8;
        }
        itemObj.GetComponent<ItemPickup>().ColliderActive(false);
        ParticleSystem.MainModule main = particle.main;
        main.startColor = new Color(color.r, color.g, color.b, 155);
        particle.gameObject.SetActive(true);
    }

    public void CheckForAnotherItem()
    {
        if(animCount < amountOfItems)
        {
            anim.SetTrigger("AnotherItem");
            InventoryManager.Instance.AddToInv(itemGained);
            animCount += 1;
        }
        else
        {
            EndOfAnimation();
        }
    }

    public void OpenBackPack()
    {
        PlayerAnimation.Instance.OpenBackPack();
    }

    public void EndOfAnimation()
    {
        InventoryManager.Instance.AddToInv(itemGained);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
