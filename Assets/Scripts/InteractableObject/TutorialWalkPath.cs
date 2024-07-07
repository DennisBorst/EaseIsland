using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWalkPath : MonoBehaviour
{
    [SerializeField] private float lerpTime;
    [SerializeField] private Transform playerNewPos;
    [SerializeField] private GameObject playerObj;
    private float timeStartedLerping;
    private float timer;
    private Vector3 playerStartPos;
    private Transform lookPos;

    public void StartWalking(Transform lookPos)
    {
        CharacterMovement.Instance.FreezePlayer(true);
        CharacterMovement.Instance.tutorialActive = true;
        this.lookPos = lookPos;
        CharacterMovement.Instance.playerObj.LookAt(this.lookPos);
        PlayerAnimation.Instance.Movement(new Vector2(1, 1), true);
        playerStartPos = CharacterMovement.Instance.gameObject.transform.position;
        timer = 0;
        timeStartedLerping = Time.time;
        StartCoroutine(UpdateInSeconds());
    }

    private void UpdateScript()
    {
        if (timer != lerpTime)
        {
            timer = LerpFloat(0, lerpTime, timeStartedLerping, lerpTime);
            CharacterMovement.Instance.gameObject.transform.position = LerpVector3(playerStartPos, playerNewPos.position, timeStartedLerping, lerpTime);
            this.gameObject.transform.LookAt(lookPos);
            playerObj.transform.localEulerAngles = new Vector3(0,0,0);
        }
        else
        {
            Debug.Log("DESTINATION REACHED");
            PlayerAnimation.Instance.Movement(new Vector2(0, 0), false);
            StopAllCoroutines();
            Destroy(this);
        }
    }

    private IEnumerator UpdateInSeconds()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            UpdateScript();
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
