using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHolderUI : MonoBehaviour
{
    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public void ChangeItem(Item item, int itemAmountNeeded)
    {
        itemImg.sprite = item.inventoryImg;
        itemAmount.text = "" + itemAmountNeeded;
        //itemAmount.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void ObjectInvisible(Item item)
    {
        ChangeItem(item, 1);
        //itemAmount.gameObject.SetActive(false);
        //this.gameObject.SetActive(false);
    }

    public void ChangeAmount(int itemAmountNeeded)
    {
        itemAmount.text = "" + itemAmountNeeded;
    }

    public void ItemTextColor(Color32 color)
    {
        itemAmount.color = color;
    }
}
