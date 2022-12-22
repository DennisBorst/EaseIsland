using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineable : MonoBehaviour
{
    [HideInInspector] public CrystalSpawner crystalSpawner;

    [SerializeField] private int health = 4;

    [SerializeField] private GameObject destroyParticle;
    [SerializeField] private GameObject mineGame;
    [SerializeField] private Item mineItem;
    [SerializeField] private GameObject itemAnim;

    [SerializeField] private Color colorPar;

    private Interactable interactable;
    private InteractableUI interactableUI;

    private bool mining = false;
    private Animator anim;

    private MineSlider mineSlider;
    private int mineValue;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (itemInHand != null)
        {
            if (itemInHand.item == ItemPickup.ItemType.Pickaxe)
            {
                if (!mining) { StartMining(); }
                else { StopMining(); }
                return;
            }
        }
    }

    public void InRange()
    {
        if (ItemInHand.Instance.currentItemSelected == null || ItemInHand.Instance.currentItemSelected.item != ItemPickup.ItemType.Pickaxe || mining) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    private void StartMining()
    {
        mining = true;
        OutRange();
        mineSlider = Instantiate(mineGame, CharacterMovement.Instance.transform.position + (Vector3.up * 2.5f), Quaternion.identity).GetComponent<MineSlider>();
        PlayerAnimation.Instance.PlayAnimCount(5);
    }

    private void StopMining()
    {
        mining = false;
        mineValue = mineSlider.MineValue();

        switch (mineValue)
        {
            case 2:
                anim.SetInteger("AnimNumber", 3);
                break;
            case 1:
                anim.SetInteger("AnimNumber", 2);
                break;
            case 0:
                anim.SetInteger("AnimNumber", 1);
                break;
            default:
                break;
        }

        StartCoroutine(DelayActions());
        PlayerAnimation.Instance.PlayAnimCount(7);
    }

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();
        anim = GetComponent<Animator>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }

    private IEnumerator DelayActions()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(mineSlider.gameObject);

        switch (mineValue)
        {
            case 2:
                Instantiate(itemAnim, transform.position, Quaternion.identity, CharacterMovement.Instance.transform).GetComponent<GainItem>().ChangeItem(mineItem, colorPar, 2);
                break;
            case 1:
                Instantiate(itemAnim, transform.position, Quaternion.identity, CharacterMovement.Instance.transform).GetComponent<GainItem>().ChangeItem(mineItem, colorPar);
                break;
            default:
                break;
        }

        health -= 1;

        if(health <= 0)
        {
            Instantiate(destroyParticle, transform.position, destroyParticle.transform.rotation);
            crystalSpawner.CrystalDestroyed();
            Destroy(this.gameObject);
        }
    }
}