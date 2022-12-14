using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildObjectInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildName;
    [SerializeField] private ItemHolderUI[] buildItemsNeeded;
    [SerializeField] private TextMeshProUGUI buildDescription;


    public void UpdateBuildItem(Building buildObject)
    {
        buildName.text = buildObject.name;
        buildName.color = Color.red;
        buildDescription.text = buildObject.description;

        for (int i = 0; i < buildItemsNeeded.Length; i++)
        {
            buildItemsNeeded[i].ItemTextColor(Color.red);
            buildItemsNeeded[i].ObjectInvisible();
        }

        int itemAvaibleCount = 0;

        for (int i = 0; i < buildObject.necessities.Length; i++)
        {
            buildItemsNeeded[i].ChangeItem(buildObject.necessities[i].item, buildObject.necessities[i].amount);
        }

        for (int i = 0; i < buildObject.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(buildObject.necessities[i].item, buildObject.necessities[i].amount))
            {
                buildItemsNeeded[i].ItemTextColor(Color.green);
                itemAvaibleCount += 1;
            }
        }

        if(buildObject.necessities.Length <= itemAvaibleCount)
        {
            buildName.color = Color.green;
        }
    }
}
