using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    [SerializeField] private Item wool;
    [SerializeField] private GameObject woolObj;
    [SerializeField] private Animator anim;
    [Space]
    [SerializeField] private float woolTimer;
    [Space]
    public NavMeshAgent agent;
    private InteractableUI interactableUI;
    public float range;
    public Transform centrePoint;

    private bool woolReady = true;

    public void GrabWool()
    {
        if (!woolReady) { return; }

        bool canBePickedUp = false;
        if (ItemInHand.Instance.currentItemSelected != null)
        {
            if (ItemInHand.Instance.currentItemSelected.item == ItemPickup.ItemType.Shears)
            {
                canBePickedUp = InventoryManager.Instance.AddToInvWithAnim(wool, 2);
                if (canBePickedUp) { PlayerAnimation.Instance.PlayAnimCount(2); }
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

        if(Vector3.Distance(this.transform.position, playerPosision) < 3.3f)
        {
            AdjustSpeed(0);
        }
        else if (Vector3.Distance(this.transform.position, playerPosision) < 8f)
        {
            Vector3 dirToPlayer = transform.position - playerPosision;
            Vector3 newPos = transform.position + dirToPlayer;
            AdjustSpeed(2.6f);
            agent.SetDestination(newPos);
        }
        else
        {
            AdjustSpeed(1.5f);
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
