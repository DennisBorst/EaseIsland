using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    public void DisableThisObject()
    {
        objectToDisable.SetActive(false);
    }
}
