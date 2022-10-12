using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour
{
    [SerializeField] private Transform canvasToRot;

    private bool loopCoroutine;

    public void InRange()
    {
        loopCoroutine = true;
        StartCoroutine(ShowToCam());
        canvasToRot.gameObject.SetActive(true);
    }

    public void OutRange()
    {
        loopCoroutine = false;
        StopCoroutine(ShowToCam());
        canvasToRot.gameObject.SetActive(false);
    }

    private void RotateToCam()
    {
        canvasToRot.LookAt(canvasToRot.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator ShowToCam()
    {
        WaitForSeconds wait = new WaitForSeconds(0.0f);

        while (loopCoroutine)
        {
            yield return wait;
            RotateToCam();
        }
    }
}