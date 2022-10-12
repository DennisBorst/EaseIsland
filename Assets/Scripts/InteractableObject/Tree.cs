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
    [SerializeField] private List<GameObject> itemDrops = new List<GameObject>();
    [SerializeField] private List<Transform> itemDropSpots = new List<Transform>();
    [Space]
    [SerializeField] private Transform treeObject;
    [SerializeField] private Transform deadParticlePos;
    [SerializeField] private ParticleSystem particleDrop;
    [SerializeField] private ParticleSystem particleDead;
    [SerializeField] private Material readyMat;
    [SerializeField] private Material unreadyMat;
    [SerializeField] private MeshRenderer Leafes;
    [Header("AppleTreeRelated")]
    [SerializeField] private bool appleTree;
    [SerializeField] private GameObject apples;
    [SerializeField] private GameObject applePrefab;

    private Animator anim;
    private Interactable interactable;
    private InteractableUI interactableUI;

    private List<Transform> currentDropSpots = new List<Transform>();
    private List<Transform> spawnDropSpots = new List<Transform>();

    private bool unableToRumble;
    private bool chopping;
    private bool treeDead = false;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (treeDead) { return; }

        if(itemInHand != null) 
        { 
            if(itemInHand.item == ItemPickup.Item.Axe)
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
        if (unableToRumble) 
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

        int randomDrop = Random.Range(minDrops, maxDrops + 1);
        anim.SetTrigger("Rumble");
        PlayerAnimation.Instance.PlayAnimCount(3);
        currentDropSpots = itemDropSpots;
        spawnDropSpots.Clear();

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
        healthTree -= 1;

        if(healthTree <= 0)
        {
            treeDead = true;
            anim.enabled = false;
            Vector3 direction = treeObject.position - playerPos;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            StartCoroutine(LerpToRotation(rotation, 3f));
        }
        else
        {
            anim.SetTrigger("Rumble");
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
        Instantiate(woodLog, this.transform.position + (Vector3.up * 0.05f), Quaternion.identity);
        Destroy(this.gameObject);
    }

    private IEnumerator WaitForParticles()
    {
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < spawnDropSpots.Count; i++)
        {
            int randomItemDrop = Random.Range(0, itemDrops.Count);
            Instantiate(itemDrops[randomItemDrop], spawnDropSpots[i].position, Quaternion.identity);
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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);

        if (appleTree)
        {
            apples.SetActive(true);
            itemDrops.Add(applePrefab);
            itemDrops.Add(applePrefab);
            itemDrops.Add(applePrefab);
        }
    }
}
