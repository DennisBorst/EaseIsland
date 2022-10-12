using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Transform canvasToRot;
    [SerializeField] private Image itemImg;

    private TextMeshProUGUI ui;
    private ItemPickup itemPickUp;
    private bool loopCoroutine;


    public void ShowUI(bool start)
    {
        if (start)
        {
            loopCoroutine = true;
            StartCoroutine(ShowToCam());
            canvasToRot.gameObject.SetActive(true);
        }
        else
        {
            loopCoroutine = false;
            StopCoroutine(ShowToCam());
            canvasToRot.gameObject.SetActive(false);
        }
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
    private void Awake()
    {
        itemPickUp = GetComponent<ItemPickup>();
        itemImg.sprite = itemPickUp.item.inventoryImg;
    }

    private void RotateToCam()
    {
        canvasToRot.LookAt(canvasToRot.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

}
