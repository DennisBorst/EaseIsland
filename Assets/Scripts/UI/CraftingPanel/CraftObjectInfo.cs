using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftObjectInfo : MonoBehaviour
{
    [HideInInspector] public CraftingPanel craftingPanel;

    [SerializeField] private TextMeshProUGUI craftName;
    [SerializeField] private ItemHolderUI[] craftItemsNeeded;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image buildItemImg;
    //[SerializeField] private Button craftButton;
    [SerializeField] private Color32 redColorTitle;
    [SerializeField] private Color32 redColor;
    [SerializeField] private Color32 greenColor;
    [SerializeField] private KeyCode holdToCraftButton;
    [SerializeField] private float craftTime;
    [SerializeField] private Image craftSlider;
    [SerializeField] private GameObject craftSliderObj;
    [SerializeField] private GameObject unableToCraftSliderObj;
    [SerializeField] private GameObject backpackFullImg;

    private bool canCraft = false;
    private Coroutine craftingRoutine;
    private float timeStartedLerping;
    private float timer;

    public void UpdateCraftItem(CraftableObject craftableObject)
    {
        StopCrafting();

        craftName.text = craftableObject.craftableItem.ItemName;
        craftName.color = redColorTitle;
        canCraft = false;
        craftSliderObj.SetActive(false);
        unableToCraftSliderObj.SetActive(true);
        itemDescription.text = craftableObject.craftableItem.itemDescription;
        buildItemImg.sprite = craftableObject.craftableItem.inventoryImg;

        for (int i = 0; i < craftItemsNeeded.Length; i++)
        {
            craftItemsNeeded[i].ItemTextColor(redColor);
            craftItemsNeeded[i].ObjectInvisible();
        }

        int itemAvaibleCount = 0;
        bool itemSlotAvailable = false;

        for (int i = 0; i < craftableObject.necessities.Length; i++)
        {
            craftItemsNeeded[i].ChangeItem(craftableObject.necessities[i].item, craftableObject.necessities[i].amount);
        }

        for (int i = 0; i < craftableObject.necessities.Length; i++)
        {
            if (InventoryManager.Instance.AmountOfItem(craftableObject.necessities[i].item, craftableObject.necessities[i].amount))
            {
                craftItemsNeeded[i].ItemTextColor(greenColor);
                itemAvaibleCount += 1;
            }

            if (InventoryManager.Instance.AmountOfItemInOneSpace(craftableObject.necessities[i].item, craftableObject.necessities[i].amount))
            {
                itemSlotAvailable = true;
            }
        }

        if(InventoryManager.Instance.AmountOfSlotsAvailable() > 0)
        {
            itemSlotAvailable = true;
        }

        if (craftableObject.necessities.Length <= itemAvaibleCount)
        {
            craftName.color = greenColor;
            backpackFullImg.SetActive(false);

            if (itemSlotAvailable)
            {
                canCraft = true;
                craftSliderObj.SetActive(true);
                unableToCraftSliderObj.SetActive(false);
            }
            else
            {
                backpackFullImg.SetActive(true);
            }
        }
    }

    public void StartCheckingInput()
    {
        StartCoroutine(CheckForInput());
    }

    public void StopCoroutines()
    {
        StopCrafting();
        StopAllCoroutines();
    }

    private void CheckInput()
    {
        if (!canCraft) { return; }

        if (Input.GetKeyDown(holdToCraftButton))
        {
            //StartCrafting
            if(craftingRoutine != null) { StopCoroutine(craftingRoutine); }
            craftingRoutine = StartCoroutine(Crafting());
        }

        if (Input.GetKeyUp(holdToCraftButton))
        {
            //StopCrafting
            StopCrafting();
        }
    }

    private void StopCrafting()
    {
        craftSlider.fillAmount = 0f;
        if (craftingRoutine == null) { return; }
        StopCoroutine(craftingRoutine);
    }

    private float LerpFloat(float start, float end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float precentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, precentageComplete);
        return result;
    }

    private IEnumerator CheckForInput()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            CheckInput();
        }
    }

    private IEnumerator Crafting()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);
        craftSlider.fillAmount = 0f;
        timer = 0;
        timeStartedLerping = Time.time;

        while (true)
        {
            yield return wait;
            if (timer != craftTime) 
            { 
                timer = LerpFloat(0, craftTime, timeStartedLerping, craftTime);
                craftSlider.fillAmount = LerpFloat(0, 1, timeStartedLerping, craftTime);
            }
            else
            {
                craftingPanel.CraftItem();
                yield break;
            }
        }
    }
}
