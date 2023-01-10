using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform dayTransform;
    [SerializeField] private Transform nightTransform;

    private NavMeshAgent agent; 

    public enum State
    {
        Idle,
        Walk
    }
    private State state;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateInSeconds());
    }

    private IEnumerator UpdateInSeconds()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            if (GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Day)
            {
                DayTime();
            }
            else
            {
                NightTime();
            }
        }
    }

    private void DayTime()
    {
        agent.SetDestination(dayTransform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    private void NightTime()
    {
        agent.SetDestination(nightTransform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
}
