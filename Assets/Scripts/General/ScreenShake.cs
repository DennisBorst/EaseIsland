using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;

    public void DoScreenShake()
    {
        cinemachineImpulseSource.GenerateImpulse();
    }

    #region Singleton
    private static ScreenShake instance;
    private void Awake()
    {
        instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public static ScreenShake Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScreenShake();
            }

            return instance;
        }
    }
    #endregion
}
