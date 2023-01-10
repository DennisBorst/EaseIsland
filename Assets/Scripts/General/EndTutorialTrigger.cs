using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject disableObject;
    [SerializeField] private GameObject enableObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            disableObject.SetActive(false);
            enableObject.SetActive(true);
        }
    }
}
