using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [SerializeField] private GameObject looseString;
    [SerializeField] private GameObject freeLooseString;
    [SerializeField] private GameObject staticString;
    [SerializeField] private GameObject firstStringObj;
    [SerializeField] private GameObject lastStringObj;

    private Transform targetPos;

    public void StartFishing(GameObject targetPos)
    {
        freeLooseString.SetActive(true);
        staticString.SetActive(false);
        this.targetPos = targetPos.transform;
    }

    public void StopFishing()
    {
        looseString.SetActive(false);
        freeLooseString.transform.eulerAngles = looseString.transform.eulerAngles;
        freeLooseString.SetActive(true);
        StopCoroutine(FishingRodUpdate());
        StartCoroutine(FishingRodEnd());
    }

    public void StartUpdateRod()
    {
        freeLooseString.SetActive(false);
        looseString.SetActive(true);

        StartCoroutine(FishingRodUpdate());
    }

    private IEnumerator FishingRodUpdate()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            UpdateObject();
        }
    }

    private IEnumerator FishingRodEnd()
    {
        yield return new WaitForSeconds(0.8f);
        staticString.SetActive(true);
        freeLooseString.SetActive(false);
    }

    private void UpdateObject()
    {
        firstStringObj.transform.LookAt(targetPos);
        lastStringObj.transform.position = targetPos.position;
    }
}
