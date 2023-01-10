using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BuildInfoButton : MonoBehaviour, ISelectHandler
{
    public BuildInfoPanel buildInfoPanel;
    public Building buildObject;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI buildName;
    [SerializeField] private Color32 unableToCraft;
    [SerializeField] private Color32 ableToCraft;

    public void OnSelect(BaseEventData eventData)
    {
        buildInfoPanel.BuildInfoSelected(buildObject, this.gameObject);
    }

    public void UpdateUI()
    {
        buildName.text = buildObject.name;
        UpdateBuilding(buildObject);
    }

    public void UpdateBuilding(Building buildObject)
    {
        buildName.color = unableToCraft;
        int itemAvaibleCount = 0;

        for (int i = 0; i < buildObject.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(buildObject.necessities[i].item, buildObject.necessities[i].amount))
            {
                itemAvaibleCount += 1;
            }
        }

        if (buildObject.necessities.Length <= itemAvaibleCount)
        {
            buildName.color = ableToCraft;
        }
    }
}
