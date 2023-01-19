using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class VibrateController : MonoBehaviour
{
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public void Vibrate(float triggerLeft, float triggerRight)
    {
        GamePad.SetVibration(playerIndex, triggerLeft, triggerRight);
    }
    private void Start()
    {
        playerIndex = PlayerIndex.One;
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
    private static VibrateController instance;
    private void Awake()
    {
        instance = this;
    }
    public static VibrateController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new VibrateController();
            }

            return instance;
        }
    }
    #endregion
}
