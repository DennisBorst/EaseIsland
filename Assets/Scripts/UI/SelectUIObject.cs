using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUIObject : MonoBehaviour
{
    public GameObject selectAsFirstUI;

    public void SelectFirstUIElement()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }

    private void OnEnable()
    {
        if(selectAsFirstUI == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }
}
