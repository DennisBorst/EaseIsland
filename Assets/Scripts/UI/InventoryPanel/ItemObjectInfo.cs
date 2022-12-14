using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObjectInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void UpdateItemInfo(Item itemObject)
    {
        itemName.text = itemObject.ItemName;
        itemDescription.text = itemObject.itemDescription;
    }
}
