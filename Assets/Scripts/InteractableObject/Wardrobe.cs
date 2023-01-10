using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private Transform playerStandPos;
    [SerializeField] private KeyCode closeMenuButton;
    [SerializeField] private KeyCode closeMenuSecondButton;
    [SerializeField] private KeyCode leftButton;
    [SerializeField] private KeyCode rightButton;
    [SerializeField] private GameObject wardrobeCanvas;

    private Interactable interactable;
    private InteractableUI interactableUI;
    private ClothUI clothUI;

    private bool inWardrobe;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (inWardrobe)
        {
            //Close wardrobe
            CloseWardrobe();
        }
        else
        {
            //Open wardrobe
            OutRange();
            OpenWardrobe();
        }
    }

    public void InRange()
    {
        if (inWardrobe) { return; }
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
        clothUI = GetComponent<ClothUI>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }

    private void OpenWardrobe()
    {
        inWardrobe = true;
        CharacterMovement.Instance.FreezePlayer(true);
        StartCoroutine(CheckForInput());
        cinemachineFreeLook.Priority = 15;

        clothUI.LoadInStats();
        playerStandPos.gameObject.SetActive(true);
        CharacterMovement.Instance.gameObject.SetActive(false);
        wardrobeCanvas.SetActive(true);
    }

    private void CloseWardrobe()
    {
        inWardrobe = false;
        StopAllCoroutines();
        wardrobeCanvas.SetActive(false);
        cinemachineFreeLook.Priority = 5;

        CharacterMovement.Instance.gameObject.transform.position = this.transform.position;
        CharacterMovement.Instance.gameObject.transform.eulerAngles = (this.transform.eulerAngles + new Vector3(0, 90, 0));

        clothUI.UpdateStats();

        CharacterMovement.Instance.gameObject.SetActive(true);
        playerStandPos.gameObject.SetActive(false);

        CharacterMovement.Instance.FreezePlayer(false);
        CharacterMovement.Instance.CloseMenu();
    }

    private void CheckInput()
    {
        Debug.Log("Search for input");

        if (Input.GetKeyDown(closeMenuButton) || Input.GetKeyDown(closeMenuSecondButton))
        {
            CloseWardrobe();
            return;
        }
        

        if (Input.GetKeyDown(leftButton))
        {
            clothUI.SwitchAccessoire(-1);
        }
        else if (Input.GetKeyDown(rightButton))
        {
            clothUI.SwitchAccessoire(1);
        }
    }

    private IEnumerator CheckForInput()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            CheckInput();
        }
    }
}
