using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private InteractableUI interactableUI;

    public void ShowText()
    {
        interactableUI.InRange();
    }

    public void HideText()
    {
        interactableUI.OutRange();
    }

    private void Awake()
    {
        interactableUI = GetComponent<InteractableUI>();
    }
}
