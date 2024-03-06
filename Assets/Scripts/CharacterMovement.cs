using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovement : MonoBehaviour
{
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public bool tutorialActive;
    public bool unableToDoStuff;
    public bool canOnlyInteract;
    private bool inventoryOpened;

    public enum PlayerState
    {
        CharacterInput,
        Menu
    }

    public PlayerState playerState;

    [Header("References")]
    public Transform playerObj;
    public Transform orientation;
    [SerializeField] private PlayerAnimation playerAnim;
    [SerializeField] private CinemachineFreeLook freelookCam;
    [SerializeField] private CinemachineFreeLook talkCam;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private FirstTimeEvent firstTime;
    [SerializeField] private GameObject walkUI;
    [SerializeField] private KeyCode pickUpButton;
    [SerializeField] private KeyCode interactButton; //Action Button
    [SerializeField] private KeyCode dropButton; //Inventory/Craft menu
    [SerializeField] private KeyCode runButton; //Run Button
    [SerializeField] private KeyCode menuButton; //Inventory/Craft menu
    [SerializeField] private KeyCode menuSecondButton; //Inventory/Craft menu
    [SerializeField] private KeyCode pauseMenuButton; 
    [SerializeField] private KeyCode leftInHandButton; 
    [SerializeField] private KeyCode rightInHandButton; 

    [Header("PlayerPreferences")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity = 9.8f;
    private float currentMovementSpeed;

    private bool isRunning;

    [Header("RangeToSpot")]
    public float spotRadius;
    public float spotAngle;
    //Items
    [SerializeField] private LayerMask itemMask;
    private bool itemInRange;
    private ItemPickup itemObj;
    private ItemInHand itemInHand;
    //Interactable
    [SerializeField] private LayerMask interactableMask;
    private bool interactableInRange;
    private Interactable interactableObj;
    //Sign
    [SerializeField] private LayerMask signMask;
    private Sign signObj;

    //[Header("TestPorpuse")]
    private InventoryManager inventoryManager;
    private CharacterController charCon;

    public void CanOnlyInteract(bool freeze)
    {
        unableToDoStuff = false;
        canOnlyInteract = freeze;

        if (freeze)
        {
            StopRunning();
        }
    }

    public void FreezePlayerForDuration(float duration)
    {
        StartCoroutine(FreezePlayerForDurationIE(duration));
    }

    public void FreezePlayer(bool freeze)
    {
        unableToDoStuff = freeze;

        if (freeze)
        {
            StopRunning();
        }
    }

    public void DecreaseMovementSpeed(float percentage)
    {
        //currentMovementSpeed = percentage * movementSpeed;
    }

    public void TalkCamActive(bool value)
    {
        if (value)
        {
            talkCam.Priority = 15;
            talkCam.gameObject.SetActive(true);
        }
        else
        {
            talkCam.gameObject.SetActive(false);
            talkCam.Priority = 5;
        }
    }

    public void OpenMenu()
    {
        playerAnim.Movement(new Vector2(0, 0), false);
        playerUI.SetActive(false);
        freelookCam.m_XAxis.m_MaxSpeed = 0f;
        isRunning = false;
        currentMovementSpeed = movementSpeed;
    }

    public void CloseMenu()
    {
        playerUI.SetActive(true);
        freelookCam.m_XAxis.m_MaxSpeed = 150f;
    }

    private void Start()
    {
        charCon = GetComponent<CharacterController>();
        inventoryManager = GetComponent<InventoryManager>();
        itemInHand = GetComponent<ItemInHand>();
        playerAnim.SetCharacterMovement(this);
        currentMovementSpeed = movementSpeed;
        playerState = PlayerState.CharacterInput;

        StartCoroutine(CheckForInteractables());
        StartCoroutine(WaitForTutorialUI());
    }

    private void OnEnable()
    {
        StartCoroutine(CheckForInteractables());
    }

    private void Update()
    {
        if (unableToDoStuff) 
        {
            if (!tutorialActive) { playerAnim.Movement(new Vector2(0, 0), false); }
            return; 
        }
        
        switch (playerState)
        {
            case PlayerState.CharacterInput:
                UpdateCharacterInput();
                break;
            case PlayerState.Menu:
                OpenMenuButton();
                break;
            default:
                break;
        }
    }

    private void UpdateCharacterInput()
    {
        Interact();
        if (canOnlyInteract) { return; }

        if (Input.GetKeyDown(pauseMenuButton))
        {
            OpenMenu();
            FreezePlayer(true);
            GameManger.Instance.EnterPauseScreen();
            return;
        }

        if (Input.GetKeyDown(menuButton) || Input.GetKeyDown(menuSecondButton))
        {
            if (inventoryManager.inventoryOpened) { return; }
            playerState = PlayerState.Menu;
            inventoryOpened = true;
            OpenMenu();
            inventoryManager.OpenInventory();
        }

        if (inventoryOpened) { return; }
        Movement();
        PickUp();
        SwitchItem();
        //DropItem();
    }

    private IEnumerator FreezePlayerForDurationIE(float duration)
    {
        unableToDoStuff = true;
        isRunning = false;
        currentMovementSpeed = movementSpeed;
        playerAnim.Movement(new Vector2(0, 0), isRunning);
        yield return new WaitForSeconds(duration);
        unableToDoStuff = false;
    }

    private IEnumerator CheckForInteractables()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        if (tutorialActive) { return; }

        ItemCheck();
        ActionCheck();
        SignCheck();
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(runButton))
        {
            if (isRunning)
            {
                StopRunning();
            }
            else
            {
                isRunning = true;
                currentMovementSpeed = runSpeed;
            }
        }

        if(new Vector2(vertical, horizontal).magnitude < 0.1f)
        {
            StopRunning();
        }

        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        moveDirection = Vector3.ClampMagnitude(moveDirection, currentMovementSpeed);
        float vSpeed = 0f;
        vSpeed -= gravity;
        moveDirection.y = vSpeed;

        playerAnim.Movement(new Vector2(vertical, horizontal), isRunning);
        charCon.Move(moveDirection * currentMovementSpeed * Time.deltaTime);

        if (!firstTime.walking && new Vector2(horizontal, vertical).magnitude > 0.1f)
        {
            StartCoroutine(TutorialUIDisappear());
        }
    }

    private void StopRunning()
    {
        isRunning = false;
        currentMovementSpeed = movementSpeed;
    }

    private void PickUp()
    {
        if (!itemInRange) { return; }

        if (Input.GetKeyDown(pickUpButton))
        {
            itemObj.PickedUp();
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(interactButton))
        {
            if (interactableInRange) 
            {
                interactableObj.Interact(itemInHand.itemStack, this.transform.position);
                return; 
            }

            if(itemInHand.itemStack == null || itemInHand.itemStack.item == null) { return; }
            if(itemInHand.itemStack.item.itemType == Item.ItemType.Food)
            {
                InventoryManager.Instance.UseFoodItem(true);
            }
        }
    }

    private void SwitchItem()
    {
        if (Input.GetKeyDown(leftInHandButton)) { itemInHand.SwitchItemInHand(-1); }
        if (Input.GetKeyDown(rightInHandButton)) { itemInHand.SwitchItemInHand(1); }
    }

    private void OpenMenuButton()
    {
        if (Input.GetKeyDown(menuButton) || Input.GetKeyDown(menuSecondButton) || Input.GetKeyDown(interactButton) || Input.GetKeyDown(pauseMenuButton))
        {
            playerState = PlayerState.CharacterInput;
            inventoryOpened = false;
            CloseMenu();
            inventoryManager.CloseInventory();
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        //{
        //    playerState = PlayerState.CharacterInput;
        //    inventoryOpened = false;
        //    CloseMenu();
        //    inventoryManager.CloseInventory();
        //    return;
        //}

        if (Input.GetKeyDown(dropButton))
        {
            inventoryManager.DropItemFromInv();
        }

        if (Input.GetKeyDown(leftInHandButton)) { inventoryManager.SwitchPanel(-1); }
        if (Input.GetKeyDown(rightInHandButton)) { inventoryManager.SwitchPanel(1); }
    }

    private void DropItem()
    {
        if (Input.GetKeyDown(dropButton) && itemInHand.itemStack.item != null)
        {
            inventoryManager.DropItemFromInv(true);
        }
    }

    private void ItemCheck()
    {
        Collider[] rangeChecks = Physics.OverlapCapsule(playerObj.transform.position + Vector3.down, playerObj.transform.position + Vector3.up, spotRadius, itemMask);

        if (rangeChecks.Length != 0)
        {
            Transform item = rangeChecks[0].transform;
            float distance = Vector3.Distance(transform.position, rangeChecks[0].transform.position);

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                float tempDis = Vector3.Distance(transform.position, rangeChecks[i].transform.position);
                if (tempDis < distance)
                {
                    item = rangeChecks[i].transform;
                }
            }

            Vector3 directionToItem = (item.position - playerObj.transform.position).normalized;

            if (Vector3.Angle(playerObj.transform.forward, directionToItem) < spotAngle / 2)
            {
                ItemPickup newItemObj = item.GetComponent<ItemPickup>();
                if(itemObj != null) 
                {
                    newItemObj.InVision(true);
                }

                if(itemObj != null && itemObj != newItemObj)
                {
                    itemObj.InVision(false);
                    newItemObj.InVision(true);
                }

                itemObj = newItemObj;
                itemInRange = true;
            }
            else
            {
                CancelItem();
                itemInRange = false;
            }
        }
        else if (itemInRange)
        {
            itemInRange = false;
        }
    }

    private void CancelItem()
    {
        if (itemObj == null) { return; }
        itemObj.InVision(false);
        itemObj = null;
    }

    private void ActionCheck()
    {
        Collider[] rangeChecks = Physics.OverlapCapsule(playerObj.transform.position + (Vector3.down * 2f), playerObj.transform.position + (Vector3.up * 2f), spotRadius, interactableMask);

        if (rangeChecks.Length != 0)
        {
            Transform interactable = rangeChecks[0].transform;
            float distance = Vector3.Distance(transform.position, rangeChecks[0].transform.position);

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                float tempDis = Vector3.Distance(transform.position, rangeChecks[i].transform.position);
                if(tempDis < distance)
                {
                    interactable = rangeChecks[i].transform;
                }
            }
            Vector3 directionToItem = (interactable.position - playerObj.transform.position).normalized;

            if (Vector3.Angle(playerObj.transform.forward, directionToItem) < spotAngle / 2)
            {
                Interactable newInteractableObj = interactable.GetComponent<Interactable>();
                if(interactableObj != null) { newInteractableObj.InRange(); }


                if (interactableObj != null && interactableObj != newInteractableObj)
                {
                    interactableObj.OutRange();
                    newInteractableObj.InRange();
                }

                interactableObj = newInteractableObj;
                interactableInRange = true;
            }
            else
            {
                if (interactableObj != null) { interactableObj.OutRange(); }
                interactableObj = null;
                interactableInRange = false;
            }
        }
        else if (interactableInRange)
        {
            if (interactableObj != null) 
            { 
                interactableObj.OutRange();
                interactableObj = null;
            }
            interactableInRange = false;
        }
    }

    private void SignCheck()
    {
        Collider[] rangeChecks = Physics.OverlapCapsule(playerObj.transform.position + (Vector3.down * 2f), playerObj.transform.position + (Vector3.up * 2f), spotRadius, signMask);

        if (rangeChecks.Length != 0)
        {
            Transform sign = rangeChecks[0].transform;
            float distance = Vector3.Distance(transform.position, rangeChecks[0].transform.position);

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                float tempDis = Vector3.Distance(transform.position, rangeChecks[i].transform.position);
                if (tempDis < distance)
                {
                    sign = rangeChecks[i].transform; ;
                }
            }
            Vector3 directionToItem = (sign.position - playerObj.transform.position).normalized;

            if (Vector3.Angle(playerObj.transform.forward, directionToItem) < spotAngle / 2)
            {
                Sign newSignObj = sign.GetComponent<Sign>();
                if (signObj != null)
                {
                    newSignObj.ShowText();
                }

                if (signObj != null && signObj != newSignObj)
                {
                    signObj.HideText();
                    newSignObj.ShowText();
                }

                signObj = newSignObj;
            }
            else
            {
                if (signObj != null) { signObj.HideText(); }
                signObj = null;
            }
        }
    }

    private IEnumerator TutorialUIDisappear()
    {
        firstTime.walking = true;
        yield return new WaitForSeconds(0.2f);
        walkUI.SetActive(false);
        Destroy(walkUI);
    }
    private IEnumerator WaitForTutorialUI()
    {
        yield return new WaitForSeconds(5f);
        if (!firstTime.walking) { walkUI.SetActive(true); }
    }

    #region Singleton
    private static CharacterMovement instance;
    private void Awake()
    {
        instance = this;
    }
    public static CharacterMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CharacterMovement();
            }

            return instance;
        }
    }
    #endregion
}
