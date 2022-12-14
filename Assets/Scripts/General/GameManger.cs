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
