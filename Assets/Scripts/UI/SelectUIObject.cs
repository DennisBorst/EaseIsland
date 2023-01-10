using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectUIObject : MonoBehaviour
{
    public GameObject selectAsFirstUI;
    public UnityEvent selectPanel;

    public void SelectFirstUIElement()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }

    public void UpdatePanel()
    {
        selectPanel.Invoke();
    }

    private void OnEnable()
    {
        if(selectAsFirstUI == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }
}
