using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPoint : MonoBehaviour
{
    [SerializeField] private GameObject fishObject;

    [SerializeField] private FishStats[] daytimeFish;
    [SerializeField] private FishStats[] nighttimeFish;

    private Interactable interactable;
    private InteractableUI interactableUI;

    private bool fishing = false;
    private bool bucketInHand;
    private Fishing fishScript;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (itemInHand != null)
        {
            if (itemInHand.item == ItemPickup.ItemType.FishingRod)
            {
                Fishing();
                return;
            }
        }
    }

    public void InRange()
    {
        if(ItemInHand.Instance.currentItemSelected == null || ItemInHand.Instance.currentItemSelected.item != ItemPickup.ItemType.FishingRod || fishing) { return; }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }

    private void Fishing()
    {
        if (!fishing)
        {
            fishing = true;
            CharacterMovement.Instance.CanOnlyInteract(true);
            Vector3 playerPos = CharacterMovement.Instance.playerObj.transform.position;
            Vector3 spawnPos = playerPos + (CharacterMovement.Instance.playerObj.transform.forward * 4f);
            spawnPos.y -= 1.3f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject fishingPool = Instantiate(fishObject, spawnPos, rotation);
            fishScript = fishingPool.GetComponent<Fishing>();

            FishStats randomFish = GetRandomFish();
            fishScript.LoadInFish(randomFish);

            OutRange();
        }
        else
        {
            fishing = false;
            fishScript.TryToCatch();
            fishScript = null;
            ItemInHand.Instance.itemObj.GetComponent<FishingRod>().StopFishing();
            CharacterMovement.Instance.CanOnlyInteract(false);
        }
    }

    private FishStats GetRandomFish()
    {
        if(GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Day)
        {
            int randomInt = Random.Range(0, daytimeFish.Length);
            return daytimeFish[randomInt];
        }
        else
        {
            int randomInt = Random.Range(0, nighttimeFish.Length);
            return nighttimeFish[randomInt];
        }
    }
}
