using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private int getOneItemValue;
    [SerializeField] private int getTwoItemsValue;

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
    }

    private void Update()
    {
        if (stop) { return; }

        if (startToEnd)
        {
            elapsedTime = Time.time - timeWeStarted;
            stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);

            //variableToLerp = Mathf.MoveTowards(0f, 100f, stepAmount);

            if(stepAmount >= slider.maxValue)
            {
                //timeWeStarted = Time.time;
                startToEnd = false;
            }
        }
        else
        {
            elapsedTime -= Time.deltaTime;
            //elapsedTime = Time.time - timeWeStarted;
            //stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);
            stepAmount = Mathf.Pow(elapsedTime * speedOfChange, exponentialModifier);

            //variableToLerp = Mathf.MoveTowards(100f, 0, stepAmount);

            if (stepAmount <= 0.6f)
            {
                timeWeStarted = Time.time;
                startToEnd = true;
            }
        }

        slider.value = stepAmount;
    }
}
