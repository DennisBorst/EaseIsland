using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BuildUI : MonoBehaviour
{
    [Header("Necessities")]
    [SerializeField] private GameObject buttonImg;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private Image[] necessitieSlots;
    [SerializeField] private TextMeshProUGUI[] amountNeeded;

    public void LoadInNecessities(Building building)
    {
        buttonImg.SetActive(false);
        buildingName.text = building.buildingName;
        buildingName.color = Color.red;

        for (int i = 0; i < necessitieSlots.Length; i++)
        {
            amountNeeded[i].color = Color.red;
            necessitieSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < building.necessities.Length; i++)
        {
            necessitieSlots[i].sprite = building.necessities[i].item.inventoryImg;
            amountNeeded[i].text = "" + building.necessities[i].amount;
            necessitieSlots[i].gameObject.SetActive(true);
        }
    }

    public void ItemTextGreen(int itemIndex)
    {
        amountNeeded[itemIndex].color = Color.green;
    }

    public void BuildTextGreen()
    {
        buttonImg.SetActive(true);
        buildingName.color = Color.green;
    }

    private void OnEnable()
    {
        StartCoroutine(ShowToCam());
    }

    private void OnDisable()
    {
        StopCoroutine(ShowToCam());
    }

    private void RotateToCam()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator ShowToCam()
    {
        WaitForSeconds wait = new WaitForSeconds(0.0f);

        while (true)
        {
            yield return wait;
            RotateToCam();
        }
    }
}
