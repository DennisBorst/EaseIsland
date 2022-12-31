using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [SerializeField] private float growTime;

    private float timer;
    private float timeStartedLerping;
    private float growBigTime;


    private void Awake()
    {
        timer = 0;
        timeStartedLerping = Time.time;
        growBigTime = growTime * 0.7f;
        StartCoroutine(GrowRoutine());

    }

    private void GrowUpdate()
    {
        if (timer != growTime)
        {
            timer = LerpFloat(0, growTime, timeStartedLerping, growTime);
            
            if(timer <= growBigTime)
            {
                this.gameObject.transform.localScale = LerpVector3(new Vector3(0, 0, 0), new Vector3(1.3f, 1.3f, 1.3f), timeStartedLerping, growBigTime);
            }
            else
            {
                this.gameObject.transform.localScale = LerpVector3(new Vector3(1.3f, 1.3f, 1.3f), new Vector3(1, 1, 1), timeStartedLerping, growTime - growBigTime);
            }
        }
        else
        {
            Destroy(this);
        }
    }

    private IEnumerator GrowRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            GrowUpdate();
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
