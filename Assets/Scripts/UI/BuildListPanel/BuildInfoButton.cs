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

    public void OnSelect(BaseEventData eventData)
    {
        buildInfoPanel.BuildInfoSelected(buildObject, this.gameObject);
    }

    public void UpdateUI()
    {
        buildName.text = buildObject.name;
    }
}
