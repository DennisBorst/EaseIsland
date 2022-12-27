using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClothChangeButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private string categoryClothName;
    [SerializeField] private TextMeshProUGUI categoryName;

    [SerializeField] private TextMeshProUGUI objectName;

    public void UpdateUI(string objectName)
    {
        this.objectName.text = objectName;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ClothManager.Instance.ClothSelected(this, this.gameObject);
    }

    void Start()
    {
        //categoryName.text = categoryClothName;
    }
}
