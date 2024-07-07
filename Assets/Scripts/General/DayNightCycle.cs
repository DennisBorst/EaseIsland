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

    [SerializeField] private bool activateOnStart = false;
    [SerializeField] private float dayDurationInMin;
    [SerializeField] private float nightDurationInMin;
    [Space]
    [SerializeField] private float morningTime;
    [SerializeField] private float noonTime;
    [Space]
    [SerializeField] private Light sunLight;
    [SerializeField] private Volume volumeDay;
    [SerializeField] private Volume volumeNight;
    [SerializeField] private GameObject sunMoonIcon;
    [SerializeField] private Vector3 rotatePointStart;
    [SerializeField] private Vector3 rotatePointEnd;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float morningEveningKelvin;
    [SerializeField] private float dayTimeKelvin;
    [Space]
    public UnityEvent dayEvent;
    public UnityEvent nightEvent;
    [Space]
    [SerializeField] private Animator fadeScreen;

    private float dayDurationInSec;
    private float nightDurationInSec;
    private float timeStartedLerping;
    private float noonTimeStartedLerping;
    private float timer;

    public delegate void StartDayTimee();
    public static StartDayTimee startDayTime;

    public delegate void StartNightTimee();
    public static StartNightTimee startNightTime;

    public void StartDayCycle()
    {
        startDayTime.Invoke();
        StartCoroutine(UpdateInSeconds());
    }

    public void StartDayTime()
    {
        timer = 0;
        timeStartedLerping = Time.time;
        noonTimeStartedLerping = timeStartedLerping + noonTime;
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
        dayDurationInSec = dayDurationInMin * 60f;
        nightDurationInSec = nightDurationInMin * 60f;
        morningTime *= dayDurationInSec;
        noonTime *= dayDurationInSec;

        startDayTime = StartDayTime;
        startNightTime = StartNightTime;

        startDayTime.Invoke();
        StartCoroutine(UpdateInSeconds());
        if (!activateOnStart) { StartCoroutine(StopCoroutineAfterTime()); }
    }

    private IEnumerator StopCoroutineAfterTime()
    {
        yield return new WaitForSeconds(0.1f);
        StopAllCoroutines();
    }

    private IEnumerator DayNightSwitchWaitTime()
    {
        dayTime = DayTime.Day;
        PlayerAnimation.Instance.Movement(new Vector2(0, 0), false);
        yield return new WaitForSeconds(1f);
        FoodManager.Instance.IncreaseFood(20f);
        startDayTime.Invoke();
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

            //whole day
            sunLight.transform.eulerAngles = LerpVector3(rotatePointStart, rotatePointEnd, timeStartedLerping, dayDurationInSec);
            sunMoonIcon.transform.eulerAngles = LerpVector3(new Vector3(0,0,0), new Vector3(0, 0, 180f), timeStartedLerping, dayDurationInSec);

            if (timer < morningTime)
            {
                //MorningTime
                volumeDay.weight = LerpFloat(0f, 1f, timeStartedLerping, morningTime);
                volumeNight.weight = LerpFloat(1f, 0f, timeStartedLerping, morningTime);
                sunLight.intensity = LerpFloat(0f, maxIntensity, timeStartedLerping, morningTime);
                sunLight.colorTemperature = LerpFloat(morningEveningKelvin, dayTimeKelvin, timeStartedLerping, morningTime);
            }
            else if(timer < noonTime)
            {
                //Daytime
                volumeDay.weight = 1f;
                volumeNight.weight = 0f;
            }
            else if(timer > noonTime)
            {
                //NoonTime
                sunLight.intensity = LerpFloat(maxIntensity, 0f, noonTimeStartedLerping, dayDurationInSec - noonTime);
                sunLight.colorTemperature = LerpFloat(dayTimeKelvin, morningEveningKelvin, noonTimeStartedLerping, dayDurationInSec - noonTime);
            }
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
            sunMoonIcon.transform.eulerAngles = LerpVector3(new Vector3(0, 0, 180f), new Vector3(0, 0, 360f), timeStartedLerping, nightDurationInSec);

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
