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
        this.gameObject.SetActive(true);
    }

    public void ObjectInvisible()
    {
        this.gameObject.SetActive(false);
    }

    public void ItemTextColor(Color32 color)
    {
        itemAmount.color = color;
    }
}
