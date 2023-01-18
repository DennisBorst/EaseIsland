using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class OnSelectButton : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/GenericClick", transform.position);
    }
}
