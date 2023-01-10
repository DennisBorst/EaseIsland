using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    [Space]
    [SerializeField] private Animator fadeScreen;
    [SerializeField] private FirstTimeEvent firstTimeCounter;

    public FMODUnity.EventReference fmodEvent;
    [SerializeField] private Location startLocation;
    private FMOD.Studio.EventInstance instanceFMOD;


    public enum Location
    {
        Island,
        Home,
        Cave
    }

    private Location currentLocationPlayer;

    public void SwitchLocation(Location location)
    {
        currentLocationPlayer = location;
        UpdateMusic();
    }

    public void UpdateMusic()
    {
        switch (currentLocationPlayer)
        {
            case Location.Island:
                if(dayNightCycle.dayTime == DayNightCycle.DayTime.Day)
                {
                    instanceFMOD.setParameterByName("Day", 1);
                    instanceFMOD.setParameterByName("Night", 0);
                    instanceFMOD.setParameterByName("Home", 0);
                    instanceFMOD.setParameterByName("Cave", 0);
                }
                else
                {
                    instanceFMOD.setParameterByName("Day", 0);
                    instanceFMOD.setParameterByName("Night", 1);
                    instanceFMOD.setParameterByName("Home", 0);
                    instanceFMOD.setParameterByName("Cave", 0);
                }
                break;
            case Location.Home:
                instanceFMOD.setParameterByName("Day", 0);
                instanceFMOD.setParameterByName("Night", 0);
                instanceFMOD.setParameterByName("Home", 1);
                instanceFMOD.setParameterByName("Cave", 0);
                break;
            case Location.Cave:
                instanceFMOD.setParameterByName("Day", 0);
                instanceFMOD.setParameterByName("Night", 0);
                instanceFMOD.setParameterByName("Home", 0);
                instanceFMOD.setParameterByName("Cave", 1);
                break;
            default:
                break;  
        }
    }

    public void SwitchDayNight()
    {
        dayNightCycle.SwitchDayNight();
    }

    public void Fade(int level)
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetInteger("FadeType", 1);
        CharacterMovement.Instance.FreezePlayerForDuration(2f);
        StartCoroutine(WaitToGoToLevel(2f, level));
    }

    public void SwitchLevel(Transform playerPos, GameObject enableLevel, GameObject disableLevel)
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetInteger("FadeType", 2);
        CharacterMovement.Instance.FreezePlayerForDuration(2f);
        StartCoroutine(SwitchLevelIE(1f, playerPos, enableLevel, disableLevel));
    }

    private IEnumerator WaitToGoToLevel(float waitTime, int level)
    {
        yield return new WaitForSeconds(waitTime);
        instanceFMOD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instanceFMOD.release();
        SceneManager.LoadScene(level);
    }

    private IEnumerator SwitchLevelIE(float waitTime, Transform playerPos, GameObject enableLevel, GameObject disableLevel)
    {
        PlayerAnimation.Instance.Movement(new Vector2(0,0), false);
        yield return new WaitForSeconds(waitTime);
        enableLevel.SetActive(true);
        disableLevel.SetActive(false);
        CharacterMovement.Instance.transform.position = playerPos.position;
    }

    void Start()
    {
        instanceFMOD = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instanceFMOD.start();
        currentLocationPlayer = startLocation;
        UpdateMusic();

        firstTimeCounter.walking = false;
        firstTimeCounter.fishing = false;
        //firstTimeCounter.mining = false;
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
