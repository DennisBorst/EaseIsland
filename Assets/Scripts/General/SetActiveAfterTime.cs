using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterTime : MonoBehaviour
{
    [SerializeField] private GameObject[] activateObjects;
    [SerializeField] private float waitTime;

    private void Awake()
    {
        StartCoroutine(waitToActivate());
    }

    private IEnumerator waitToActivate()
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < activateObjects.Length; i++)
        {
            activateObjects[i].SetActive(true);
        }
        Destroy(this);
    }
}
