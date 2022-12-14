using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour
{
    public enum DayTime
    {
        Day,
        Night
    }
    [HideInInspector] public DayTime dayTime;

    [SerializeField] private float dayDurationInMin;
    [SerializeField] private float nightDurationInMin;
    [Space]
    [SerializeField] private Light sunLight;
    [SerializeField] private Volume volumeDay;
    [SerializeField] private Volume volumeNight;
    [SerializeField] private Vector3 rotatePointStart;
    [SerializeField] private Vector3 rotatePointEnd;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float morningEveningKelvin;
    [SerializeField] private float dayTimeKelvin;
    [Space]
    [SerializeField] private UnityEvent dayEvent;
    [SerializeField] private UnityEvent nightEvent;
    [Space]
    [SerializeField] private Animator fadeScreen;

    private float dayDurationInSec;
    private float nightDurationInSec;
    private float timeStartedLerping;
    private float timer;

    public delegate void StartDayTimee();
    public static StartDayTimee startDayTime;

    public delegate void StartNightTimee();
    public static StartNightTimee startNightTime;

    public void StartDayTime()
    {
        timer = 0;
        timeStartedLerping = Time.time;
        dayTime = DayTime.Day;
        dayEvent.Invoke();
    }

    public void StartNightTime()
    {
        timer = 0;
        timeStartedLerping = Time.time;
        dayTime = DayTime.Night;
        nightEvent.Invoke();
    }

    public void SwitchDayNight()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetInteger("FadeType", 2);
        CharacterMovement.Instance.FreezePlayerForDuration(2f);
        StartCoroutine(DayNightSwitchWaitTime());
    }

    private void Start()
    {
        dayTime = DayTime.Day;
        StartCoroutine(UpdateInSeconds());
        dayDurationInSec = dayDurationInMin * 60f;
        nightDurationInSec = nightDurationInMin * 60f;
        timeStartedLerping = Time.time;

        startDayTime = StartDayTime;
        startNightTime = StartNightTime;
    }

    private IEnumerator DayNightSwitchWaitTime()
    {
        yield return new WaitForSeconds(1f);
        FoodManager.Instance.IncreaseFood(20f);
        timer = 0;
        timeStartedLerping = Time.time;
        dayTime = DayTime.Day;
        dayEvent.Invoke();
    }

    private IEnumerator UpdateInSeconds()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            if (dayTime == DayTime.Day)
            {
                DayTimeCount();
            }
            else
            {
                NightTimeCount();
            }
        }
    }

    private void DayTimeCount()
    {
        if (timer != dayDurationInSec)
        {
            timer = LerpFloat(0, dayDurationInSec, timeStartedLerping, dayDurationInSec);
            sunLight.transform.eulerAngles = LerpVector3(rotatePointStart, rotatePointEnd, timeStartedLerping, dayDurationInSec);
            sunLight.intensity = LerpFloat(0f, maxIntensity, timeStartedLerping, dayDurationInSec / 4);
            sunLight.colorTemperature = LerpFloat(morningEveningKelvin, dayTimeKelvin, timeStartedLerping, dayDurationInSec / 4);
            volumeDay.weight = LerpFloat(0f, 1f, timeStartedLerping, dayDurationInSec / 8);
            volumeNight.weight = LerpFloat(1f, 0f, timeStartedLerping, dayDurationInSec / 8);
        }
        else
        {
            startNightTime.Invoke();
            //StartNightTime();
        }
    }

    private void NightTimeCount()
    {
        if (timer != nightDurationInSec)
        {

            timer = LerpFloat(0, nightDurationInSec, timeStartedLerping, nightDurationInSec);
            sunLight.intensity = LerpFloat(maxIntensity, 0f, timeStartedLerping, dayDurationInSec / 4);

            sunLight.colorTemperature = LerpFloat(dayTimeKelvin, morningEveningKelvin, timeStartedLerping, dayDurationInSec / 8);
            volumeDay.weight = LerpFloat(1f, 0f, timeStartedLerping, nightDurationInSec / 8);
            volumeNight.weight = LerpFloat(0f, 1f, timeStartedLerping, nightDurationInSec / 8);
        }
        else
        {
            startDayTime.Invoke();
            //StartDayTime();
        }
    }

    private Vector3 LerpVector3(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float precentageComplete = timeSinceStarted / lerpTime;

        Vector3 result = Vector3.Lerp(start, end, precentageComplete);
        return result;
    }

    private float LerpFloat(float start, float end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float precentageComplete = timeSinceStarted / lerpTime;

        float result = Mathf.Lerp(start, end, precentageComplete);
        return result;
    }
}
