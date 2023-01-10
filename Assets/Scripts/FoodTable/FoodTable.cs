using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodTable : MonoBehaviour
{
    [SerializeField] private KeyCode closeMenuButton;
    [SerializeField] private KeyCode closeMenuSecondButton;
    [Space]
    [SerializeField] private GameObject foodCanvas;
    [SerializeField] private FoodTableUI foodTableUI;
    [SerializeField] private Slider foodSlider;

    private bool menuOpened;
    private Interactable interactable;
    private InteractableUI interactableUI;
    private bool clothReady = false;
    private Clothing cloth;
    private Coroutine checkForInputRoutine;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Night && !menuOpened) { return; }

        if (menuOpened)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void InRange()
    {
        if (menuOpened || GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Night) 
        {
            OutRange();
            return; 
        }
        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }



    public void ClothReady(Clothing cloth, float waitTime)
    {
        clothReady = true;
        this.cloth = cloth;

        StartCoroutine(WaitForNewClothing(waitTime));
    }


    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);

        foodTableUI.foodTable = this;
    }

    private void Start()
    {
        FoodManager.Instance.AddFoodTable(foodTableUI);
        FoodManager.Instance.StartFoodTimer();

        FoodManager.Instance.AddSlider(foodSlider);
    }

    private void OpenMenu()
    {
        menuOpened = true;

        CharacterMovement.Instance.OpenMenu();
        CharacterMovement.Instance.FreezePlayer(true);

        foodCanvas.SetActive(true);
        foodTableUI.GetInventoryItems();

        if (checkForInputRoutine != null) { StopCoroutine(checkForInputRoutine); }
        checkForInputRoutine = StartCoroutine(CheckForInput());
    }

    private void CloseMenu()
    {
        menuOpened = false;
        StopCoroutine(checkForInputRoutine);

        foodCanvas.SetActive(false);
        foodTableUI.UpdateInventory();

        CharacterMovement.Instance.FreezePlayer(false);
        CharacterMovement.Instance.CloseMenu();

        if (clothReady)
        {
            clothReady = false;
            ClothManager.Instance.AddCloth(cloth);
            cloth = null;
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(closeMenuButton) || Input.GetKeyDown(closeMenuSecondButton))
        {
            CloseMenu();
        }
    }

    private IEnumerator WaitForNewClothing(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foodTableUI.ClothSliderActivate();
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
