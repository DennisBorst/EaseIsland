using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private int getOneItemValue;
    [SerializeField] private int getTwoItemsValue;
    [Space]
    [SerializeField] private FirstTimeEvent firstTimeCounter;
    [SerializeField] private GameObject tutorialCloud;

    private float lerpValue;
    private float timeWeStarted;

    public float speedOfChange = 2f;
    public float exponentialModifier = 10f;

    private bool startToEnd = true;
    private float variableToLerp;

    private float elapsedTime;
    private float stepAmount;

    private bool stop = false;

    public int MineValue()
    {
        stop = true;

        if (slider.value >= getOneItemValue) { firstTimeCounter.mining = true; }

        if (slider.value >= getTwoItemsValue) { return 2; }
        else if (slider.value >= getOneItemValue) { return 1; }
        else { return 0; }
    }

    private void Awake()
    {
        slider.maxValue = 100f;
        slider.value = 0f;

        timeWeStarted = Time.time;
        elapsedTime = 0.4f;

        if (!firstTimeCounter.mining) { tutorialCloud.SetActive(true); }
    }

    private void Update()
    {
        if (stop) { return; }

        if (startToEnd)
        {
            elapsedTime = Time.time - timeWeStarted;
            stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);
            float vibrateamount = stepAmount / 35f;
            VibrateController.Instance.Vibrate(0.0118001f * vibrateamount, 0.0118001f * vibrateamount);

            if (stepAmount >= slider.maxValue)
            {
                //timeWeStarted = Time.time;
                startToEnd = false;
            }
        }
        else
        {
            elapsedTime -= Time.deltaTime;
            stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);
            float vibrateamount = stepAmount / 35f;
            VibrateController.Instance.Vibrate(0.0118001f * vibrateamount, 0.0118001f * vibrateamount);

            if (stepAmount <= 0.6f)
            {
                timeWeStarted = Time.time;
                startToEnd = true;
            }
        }

        slider.value = stepAmount;
    }
}
