using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OptionButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private GameObject moveVisImg;

    public void ChangeName(string option)
    {
        optionText.text = option;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Selected(true);
        DialogueManager.Instance.OptionSelected(this);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/GenericClick", transform.position);
    }

    public void Selected(bool value)
    {
        moveVisImg.SetActive(value);
    }
}
