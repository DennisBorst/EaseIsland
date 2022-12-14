using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUIObject : MonoBehaviour
{
    public GameObject selectAsFirstUI;

    public void SelectFirstUIElemnt()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }

    private void Start()
    {
        if(selectAsFirstUI == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectAsFirstUI);
    }
}
