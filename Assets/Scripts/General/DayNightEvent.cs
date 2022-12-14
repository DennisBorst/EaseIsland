using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent dayEvent;
    [SerializeField] private UnityEvent nightEvent;

    private void Awake()
    {
        DayNightCycle.startDayTime += StartDayEvent;
        DayNightCycle.startDayTime += StartNightEvent;

        //dayEvent.AddListener(GameManger.Instance.dayNightCycle.StartDayTime);
        //nightEvent.AddListener(GameManger.Instance.dayNightCycle.StartNightTime);
    }

    private void StartDayEvent()
    {
        dayEvent.Invoke();
    }

    private void StartNightEvent()
    {
        nightEvent.Invoke();
    }
}
