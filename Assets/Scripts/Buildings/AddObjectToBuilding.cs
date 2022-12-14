using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToBuilding : MonoBehaviour
{
    [SerializeField] private Transform buildLocation;
    [SerializeField] private Building building;
    [Space]
    [SerializeField] private ParticleSystem buildComplete;
    [SerializeField] private bool destroyAfterBuild = true;
    [SerializeField] private GameObject alwaysDestroyedAfterBuild;
    private BuildUI buildUI;
    private bool canBuild = false;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (!canBuild) { return; }

        if(itemInHand.itemType == Item.ItemType.Crystal)
        {
            Collider colItem = Instantiate(itemInHand.prefabItem, buildLocation.transform.position, buildLocation.transform.rotation).GetComponent<Collider>();
            colItem.enabled = false;
            Instantiate(buildComplete, buildLocation.transform.position, Quaternion.identity);
            InventoryManager.Instance.UseItemFromHand();
        }


        if (destroyAfterBuild)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.layer = 0;
            Interactable interactable = GetComponent<Interactable>();
            Destroy(interactable);
            Destroy(alwaysDestroyedAfterBuild);
            OutRange();
            Destroy(this);
        }
    }

    public void InRange()
    {
        CheckItems();
        buildUI.gameObject.SetActive(true);
    }

    public void OutRange()
    {
        buildUI.gameObject.SetActive(false);
    }

    private void Awake()
    {
        buildUI = GetComponentInChildren<BuildUI>();
        CheckItems();
    }

    private void CheckItems()
    {
        buildUI.LoadInNecessities(building);

        if (ItemInHand.Instance.currentItemSelected != null && ItemInHand.Instance.currentItemSelected.itemType == Item.ItemType.Crystal) 
        {
            canBuild = true;
            buildUI.BuildTextGreen();
        }
        else
        {
            canBuild = false;
        }

        //int necessitieChecker = 0;

        //for (int i = 0; i < building.necessities.Length; i++)
        //{
        //    if (InventoryManager.Instance.AmountOfItem(building.necessities[i].item, building.necessities[i].amount))
        //    {
        //        necessitieChecker += 1;
        //        buildUI.ItemTextGreen(i);
        //    }
        //}

        //if (building.necessities.Length <= necessitieChecker)
        //{
        //    canBuild = true;
        //    buildUI.BuildTextGreen();
        //}
        //else
        //{
        //    canBuild = false;
        //}
    }
}
