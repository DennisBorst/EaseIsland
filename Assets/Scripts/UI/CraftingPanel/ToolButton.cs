using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ToolButton : MonoBehaviour, ISelectHandler
{
    public CraftingPanel craftingPanel;
    public CraftableObject craftObject;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI craftName;

    public void OnSelect(BaseEventData eventData)
    {
        craftingPanel.ToolSelected(craftObject, this.gameObject);
    }

    public void UpdateUI()
    {
        craftName.text = craftObject.craftableItem.ItemName;
    }
}
