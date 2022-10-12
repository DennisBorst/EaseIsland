using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
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
    [SerializeField] private Vector3 rotatePointStart;
    [SerializeField] private Vector3 rotatePointEnd;
    [SerializeField] private float maxIntensity;
    [Space]
    [SerializeField] private UnityEvent dayEvent;
    [SerializeField] private UnityEvent nightEvent;
    [Space]
    [SerializeField] private Animator fadeScreen;

    private float dayDurationInSec;
    private float nightDurationInSec;
    private float timeStartedLerping;
    private float timer;


    public void SwitchDayNight()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetInteger("FadeType", 2);
        CharacterMovement.Instance.FreezePlayerForDuration(2f);
        StartCoroutine(DayNightSwitchWaitTime());
    }

    public void Fade(int level)
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetInteger("FadeType", 1);
        CharacterMovement.Instance.FreezePlayerForDuration(2f);
        StartCoroutine(WaitToGoToLevel(2f, level));
    }

    private void Start()
    {
        dayTime = DayTime.Day;
        StartCoroutine(UpdateInSeconds());
        dayDurationInSec = dayDurationInMin * 60f;
        nightDurationInSec = nightDurationInMin * 60f;
        timeStartedLerping = Time.time;
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

    private IEnumerator WaitToGoToLevel(float waitTime, int level)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(level);
    }

    private void DayTimeCount()
    {
        if(timer != dayDurationInSec)
        {
            timer = LerpFloat(0, dayDurationInSec, timeStartedLerping, dayDurationInSec);
            sunLight.transform.eulerAngles = LerpVector3(rotatePointStart, rotatePointEnd, timeStartedLerping, dayDurationInSec);
            sunLight.intensity = LerpFloat(0, maxIntensity, timeStartedLerping, dayDurationInSec / 4);
        }
        else
        {
            timer = 0;
            timeStartedLerping = Time.time;
            dayTime = DayTime.Night;
            nightEvent.Invoke();
        }
    }

    private void NightTimeCount()
    {
        if (timer != nightDurationInSec)
        {
            timer = LerpFloat(0, nightDurationInSec, timeStartedLerping, nightDurationInSec);
            sunLight.intensity = LerpFloat(maxIntensity, 0f, timeStartedLerping, nightDurationInSec / 5);
        }
        else
        {
            timer = 0;
            timeStartedLerping = Time.time;
            dayTime = DayTime.Day;
            dayEvent.Invoke();
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

    #region Singleton
    private static GameManger instance;
    private void Awake()
    {
        instance = this;
    }
    public static GameManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManger();
            }

            return instance;
        }
    }
    #endregion
}
