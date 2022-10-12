using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : MonoBehaviour
{
     private InteractableUI interactableUI;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if(GameManger.Instance.dayTime == GameManger.DayTime.Night)
        {
            GameManger.Instance.SwitchDayNight();
        }
    }

    public void InRange()
    {
        if (GameManger.Instance.dayTime != GameManger.DayTime.Night) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    private void Awake()
    {
        interactableUI = GetComponent<InteractableUI>();
    }
}
