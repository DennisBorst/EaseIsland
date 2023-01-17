using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotFarmer : MonoBehaviour
{
    [SerializeField] private KeyCode closeMenuButton;
    [SerializeField] private KeyCode closeMenuSecondButton;
    [SerializeField] private KeyCode holdToCollectButton;

    [SerializeField] private GameObject carrotCanvas;
    [SerializeField] private TradeUIManager tradeUIManager;
    [Header("Collect Slider")]
    [SerializeField] private float collectTime;
    [SerializeField] private Image collectSlider;
    [SerializeField] private GameObject collectSliderObj;
    [SerializeField] private GameObject unableToCollectSliderObj;


    private Coroutine collectRoutine;
    private float timeStartedLerping;
    private float timer;
    private bool canCollect = false;

    public void OpenMenu()
    {
        DialogueManager.Instance.CloseMenu();

        carrotCanvas.SetActive(true);
        tradeUIManager.GetInventoryItems();

        StartCoroutine(CheckForInput());
    }

    public void CanCollect(bool value)
    {
        canCollect = value;
        collectSliderObj.SetActive(value);
        unableToCollectSliderObj.SetActive(!value);
    }

    private void Awake()
    {
        tradeUIManager.carrotFarmer = this;
    }

    private void CloseMenu()
    {
        StopAllCoroutines();

        tradeUIManager.CollectItems(true);
        carrotCanvas.SetActive(false);
        tradeUIManager.UpdateInventory();
        DialogueManager.Instance.EndConversation();
    }

    private void StopCollecting()
    {
        collectSlider.fillAmount = 0f;
        if (collectRoutine == null) { return; }
        StopCoroutine(collectRoutine);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(closeMenuButton) || Input.GetKeyDown(closeMenuSecondButton))
        {
            CloseMenu();
        }

        if (!canCollect) { return; }

        if (Input.GetKeyDown(holdToCollectButton))
        {
            //StartCollecting
            if (collectRoutine != null) { StopCoroutine(collectRoutine); }
            collectRoutine = StartCoroutine(Collecting());
        }

        if (Input.GetKeyUp(holdToCollectButton))
        {
            //StopCollecting
            StopCollecting();
        }
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

    private IEnumerator Collecting()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);
        collectSlider.fillAmount = 0f;
        timer = 0;
        timeStartedLerping = Time.time;

        while (true)
        {
            yield return wait;
            if (timer != collectTime)
            {
                timer = LerpFloat(0, collectTime, timeStartedLerping, collectTime);
                collectSlider.fillAmount = LerpFloat(0, 1, timeStartedLerping, collectTime);
            }
            else
            {
                tradeUIManager.CollectItems(false);
                yield break;
            }
        }
    }
}
