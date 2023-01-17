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

    private ClothUI clothUI;

    public void UpdateUI(string objectName)
    {
        this.objectName.text = objectName;
    }

    public void OnSelect(BaseEventData eventData)
    {
        clothUI.ClothSelected(this, this.gameObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/GenericClick", transform.position);
    }

    public void AddClothUI(ClothUI clothUI)
    {
        this.clothUI = clothUI;
    }

    void Start()
    {
        //categoryName.text = categoryClothName;
    }
}
