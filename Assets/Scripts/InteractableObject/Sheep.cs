using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    [SerializeField] private Item wool;
    [SerializeField] private GameObject woolObj;
    [SerializeField] private Animator anim;
    [Space]
    [SerializeField] private float woolTimer;
    [Space]
    [SerializeField] private ParticleSystem runPar;

    private InteractableUI interactableUI;
    private bool woolReady = true;
    private bool running = true;

    public void GrabWool()
    {
        if (!woolReady) { return; }

        bool canBePickedUp = false;
        if (ItemInHand.Instance.currentItemSelected != null)
        {
            if (ItemInHand.Instance.currentItemSelected.item == ItemPickup.ItemType.Shears)
            {
                canBePickedUp = InventoryManager.Instance.AddToInvWithAnim(wool, 2);
                if (canBePickedUp) 
                { 
                    PlayerAnimation.Instance.PlayAnimCount(2);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Activities/Shears", transform.position);
                }
            }
            else
            {
                canBePickedUp = InventoryManager.Instance.AddToInvWithAnim(wool);
                if (canBePickedUp) { PlayerAnimation.Instance.PlayAnimCount(4); }
            }
        }

        if (canBePickedUp)
        {
            StartCoroutine(CooldownWoolReset());
            OutRange();
        }
    }

    public void InRange()
    {
        if (!woolReady || !InventoryManager.Instance.CheckSpace(wool)) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    public void AdjustSpeed(float speed)
    {
        agent.speed = speed;

        if (speed == 0)
        {
            anim.SetBool("idle", true);
            anim.SetBool("running", false);
        }
        else if(speed < 2f)
        {
            anim.SetBool("idle", false);
            anim.SetBool("running", false);
        }
        else
        {
            anim.SetBool("idle", false);
            anim.SetBool("running", true);
        }
    }

    private IEnumerator CooldownWoolReset()
    {
        woolReady = false;
        woolObj.SetActive(false);
        yield return new WaitForSeconds(woolTimer);
        woolReady = true;
        woolObj.SetActive(true);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        interactableUI = GetComponent<InteractableUI>();
    }


    private void Update()
    {
        Vector3 playerPosision = CharacterMovement.Instance.transform.position;

        if (Vector3.Distance(this.transform.position, playerPosision) < 3.4f)
        {
            AdjustSpeed(0);
            runPar.Stop();
        }
        else if (running)
        {
            AdjustSpeed(3.4f);

            if (!runPar.isEmitting) { runPar.Play(); }

            Vector3 normDir = (transform.position - playerPosision).normalized;
            //normDir = Quaternion.AngleAxis(Random.Range(45, 135), Vector3.up) * normDir;
            Vector3 newPos = transform.position + (normDir * 2f);
            agent.SetDestination(newPos);
            agent.isStopped = false;

            if (Vector3.Distance(this.transform.position, playerPosision) > 9f)
            {
                running = false;
            }
        }
        else
        {

            if (Vector3.Distance(this.transform.position, playerPosision) < 9f)
            {
                //Vector3 dirToPlayer = transform.position - playerPosision;
                //Vector3 newPos = transform.position + dirToPlayer;
                running = true;
                AdjustSpeed(3.4f);
                if (!runPar.isEmitting) { runPar.Play(); }
            }
            else
            {
                AdjustSpeed(1.5f);
                runPar.Stop();
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
    }

    private Vector3 GetRandomRunAwayPoint()
    {
        Vector3 playerPos = CharacterMovement.Instance.transform.position;
        //Vector3 forwardDirection = CharacterMovement.Instance.transform.forward;

        //Vector3 randomDirection = centrePoint.position + Random.insideUnitSphere * 10;
        //NavMeshHit hit;
        //Vector3 dirToPlayer = (centrePoint.position - playerPos) * 1f;
        //NavMesh.SamplePosition(playerPos + dirToPlayer, out hit, 100, NavMesh.AllAreas);
        //NavMeshHit hit;
        //NavMesh.FindClosestEdge(playerPos + dirToPlayer, out hit, NavMesh.AllAreas);
        Vector3 normDir = (playerPos + this.transform.position).normalized;
        //Vector3 pointOnNavMesh = hit.position;

        return normDir;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
