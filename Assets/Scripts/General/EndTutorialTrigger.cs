using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorialTrigger : MonoBehaviour
{
    [SerializeField] private NPC grandpa;
    [SerializeField] private TutorialWalkPath playerWalkPath;
    [SerializeField] private GameObject disableObject;
    [SerializeField] private GameObject enableObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            grandpa.ChangeState(StateEnum.Tutorial);
            playerWalkPath.StartWalking(grandpa.gameObject.transform);
            GameManger.Instance.dayNightCycle.StartDayCycle();
            StartCoroutine(WaitToDestroy());
        }
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(6f);
        grandpa.AddDayNightTimeEvent();
        CharacterMovement.Instance.tutorialActive = false;
        disableObject.SetActive(false);
        enableObject.SetActive(true);
        FoodManager.Instance.StartFoodTimer();
        Destroy(this.gameObject);
    }
}
