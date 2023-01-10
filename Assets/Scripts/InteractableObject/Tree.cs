using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int minDrops;
    [SerializeField] private int maxDrops;
    [SerializeField] private float cooldownRumble;
    [SerializeField] private int healthTree;
    [Space]
    [SerializeField] private GameObject woodLog;
    [SerializeField] private GameObject treeSteadyObject;
    [SerializeField] private GameObject treeFallObject;
    [SerializeField] private List<GameObject> itemDrops = new List<GameObject>();
    [SerializeField] private Transform[] itemDropSpots;
    [Space]
    [SerializeField] private Transform treeObject;
    [SerializeField] private Transform deadParticlePos;
    [SerializeField] private ParticleSystem particleDrop;
    [SerializeField] private ParticleSystem particleDead;
    [SerializeField] private Material readyMat;
    [SerializeField] private Material unreadyMat;
    [SerializeField] private MeshRenderer Leafes;

    [Header("TreeChopped")]
    [SerializeField] private float timeToRecover;
    [SerializeField] private ParticleSystem recoverPar;

    [Header("AppleTreeRelated")]
    [SerializeField] private bool appleTree;
    [SerializeField] private GameObject apples;
    [SerializeField] private GameObject applePrefab;

    private Animator anim;
    private Interactable interactable;
    private InteractableUI interactableUI;
    private int currentHealthTree;

    private List<Transform> currentDropSpots = new List<Transform>();
    private List<Transform> spawnDropSpots = new List<Transform>();

    private Rigidbody rbFallPart;
    private bool unableToRumble;
    private bool chopping;
    private bool treeDead = false;

    private Vector3 treeRot;
    private GameObject woodLogObj;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (treeDead) { return; }

        if(itemInHand != null) 
        { 
            if(itemInHand.item == ItemPickup.ItemType.Axe)
            {
                PlayerAnimation.Instance.PlayAnimCount(itemInHand.animNumber);
                Chop(playerPos);
                return;
            }
        }

        RumbleTree();
    }

    public void InRange()
    {
        if (unableToRumble && ItemInHand.Instance.currentItemSelected.item != ItemPickup.ItemType.Axe || treeDead) 
        {
            interactableUI.OutRange();
            return; 
        }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    public void RumbleTree()
    {
        if (unableToRumble) { return; }
        StartCoroutine(RumbleCooldown());

        CheckSpawnPoints();
        int randomDrop = Random.Range(minDrops, maxDrops + 1);
        if(randomDrop > currentDropSpots.Count) { randomDrop = currentDropSpots.Count; }

        anim.SetTrigger("Rumble");
        PlayerAnimation.Instance.PlayAnimCount(3);

        for (int i = 0; i < randomDrop; i++)
        {
            int randomSpawnDrop = Random.Range(0, currentDropSpots.Count);
            Instantiate(particleDrop, currentDropSpots[randomSpawnDrop].position + (Vector3.up * 3f), particleDrop.transform.rotation);
            spawnDropSpots.Add(currentDropSpots[randomSpawnDrop]);
            currentDropSpots.RemoveAt(randomSpawnDrop);
        }
        StartCoroutine(WaitForParticles());
    }

    private void Chop(Vector3 playerPos)
    {
        currentHealthTree -= 1;

        if(currentHealthTree <= 0)
        {
            treeDead = true;
            anim.enabled = false;
            treeSteadyObject.SetActive(false);
            treeFallObject.SetActive(true);
            StartCoroutine(RecoverCooldown());

            Vector3 direction = treeObject.position - playerPos;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            StartCoroutine(LerpToRotation(rotation, 3f));

        }
        else
        {
            anim.SetTrigger("Rumble");
        }
    }

    private void CheckSpawnPoints()
    {
        currentDropSpots.Clear();
        spawnDropSpots.Clear();

        for (int i = 0; i < itemDropSpots.Length; i++)
        {
            if (itemDropSpots[i].transform.childCount == 0)
            {
                currentDropSpots.Add(itemDropSpots[i]);
            }
        }
    }

    private IEnumerator LerpToRotation(Quaternion rotation, float lerpDuration)
    {
        float timeElapsed = 0;
        while (Quaternion.Angle(treeObject.rotation, rotation) > 1f)
        {
            treeObject.rotation = Quaternion.Lerp(treeObject.rotation, rotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Instantiate(particleDead, deadParticlePos.position + (Vector3.up * 2f), Quaternion.identity);
        woodLogObj = Instantiate(woodLog, treeObject.transform.position + (Vector3.up * 0.04f), Quaternion.identity);
        treeObject.gameObject.SetActive(false);
    }

    private IEnumerator WaitForParticles()
    {
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < spawnDropSpots.Count; i++)
        {
            int randomItemDrop = Random.Range(0, itemDrops.Count);
            Instantiate(itemDrops[randomItemDrop], spawnDropSpots[i].position, Quaternion.identity, spawnDropSpots[i]);
        }
    }

    private IEnumerator RumbleCooldown()
    {
        unableToRumble = true;
        Leafes.material = unreadyMat;
        yield return new WaitForSeconds(cooldownRumble);
        unableToRumble = false;
        Leafes.material = readyMat;
    }

    private IEnumerator RecoverCooldown()
    {
        currentHealthTree = healthTree;
        yield return new WaitForSeconds(timeToRecover);
        if(woodLogObj != null) { Destroy(woodLogObj); }

        treeDead = false;
        anim.enabled = true;
        treeObject.transform.eulerAngles = treeRot;
        treeObject.gameObject.SetActive(true);
        treeSteadyObject.SetActive(true);
        treeFallObject.SetActive(false);
        recoverPar.Play();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();
        rbFallPart = GetComponentInChildren<Rigidbody>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
        currentHealthTree = healthTree;
        treeRot = treeObject.transform.eulerAngles;

        if (appleTree)
        {
            apples.SetActive(true);
            itemDrops.Add(applePrefab);
            itemDrops.Add(applePrefab);
            itemDrops.Add(applePrefab);
        }
    }
}
