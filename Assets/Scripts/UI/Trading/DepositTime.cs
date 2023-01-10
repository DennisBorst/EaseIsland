using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepositTime : MonoBehaviour
{
    [SerializeField] private float timeToDeposit;
    [SerializeField] private Image imgSlider;

    private ItemTradeButton itemTradeButton;
    private float timeStartedLerping;
    private float timer;
    private bool deposit;

    public void WaitToDeposit()
    {
        timeStartedLerping = Time.time;
        timer = 0f;
        deposit = false;
        StartCoroutine(UpdateSlider());
    }


    private void Awake()
    {
        itemTradeButton = GetComponent<ItemTradeButton>();
        itemTradeButton.onClick += WaitToDeposit;
    }

    private IEnumerator UpdateSlider()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            UpdateItemSlider();
        }
    }

    private void UpdateItemSlider()
    {
        if (itemTradeButton.tradeUIManager != null && itemTradeButton.tradeUIManager.movingItem == itemTradeButton.itemStack || itemTradeButton.itemStack.item.item == ItemPickup.ItemType.Empty)
        {
            DisableUI();
            return;
        }

        if (itemTradeButton.foodTable != null && itemTradeButton.foodTable.movingItem == itemTradeButton.itemStack || itemTradeButton.itemStack.item.item == ItemPickup.ItemType.Empty)
        {
            DisableUI();
            return;
        }

        timer = LerpFloat(0, timeToDeposit, timeStartedLerping, timeToDeposit);
        imgSlider.fillAmount = LerpFloat(0, 1, timeStartedLerping, timeToDeposit);

        if (timer == timeToDeposit && !deposit)
        {
            deposit = true;
            if (itemTradeButton.tradeUIManager != null) { itemTradeButton.tradeUIManager.DepositItem(itemTradeButton); }
            if (itemTradeButton.foodTable != null) { itemTradeButton.foodTable.DepositItem(itemTradeButton); }
            DisableUI();
        }
    }

    private void DisableUI()
    {
        StopCoroutine(UpdateSlider());
        imgSlider.fillAmount = 0;
    }

    private void OnDisable()
    {
        if(itemTradeButton.tradeUIManager != null)
        {
            if (itemTradeButton.tradeUIManager.movingItem == itemTradeButton.itemStack || itemTradeButton.itemStack.item.item == ItemPickup.ItemType.Empty) { return; }
            itemTradeButton.tradeUIManager.DepositItem(itemTradeButton);
        }

        if(itemTradeButton.foodTable != null)
        {
            if (itemTradeButton.foodTable.movingItem == itemTradeButton.itemStack || itemTradeButton.itemStack.item.item == ItemPickup.ItemType.Empty) { return; }
            itemTradeButton.foodTable.DepositItem(itemTradeButton);
        }

        deposit = true;
        DisableUI();
    }

    private float LerpFloat(float start, float end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float precentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, precentageComplete);
        return result;
    }
}
