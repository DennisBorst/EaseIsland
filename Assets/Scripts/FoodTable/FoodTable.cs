using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodTable : MonoBehaviour
{
    [SerializeField] private GameObject foodCanvas;
    [SerializeField] private FoodTableUI foodTableUI;
    [SerializeField] private Slider foodSlider;

    private bool menuOpened;
    private Interactable interactable;
    private InteractableUI interactableUI;
    private bool clothReady = false;
    private Clothing cloth;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if (GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Night && !menuOpened) { return; }

        if (menuOpened)
        {
            CloseMenu();
        }
        else
        {
            menuOpened = true;
            CharacterMovement.Instance.OpenMenu();
            CharacterMovement.Instance.CanOnlyInteract(true);
            foodCanvas.SetActive(true);
            foodTableUI.GetInventoryItems();
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

    public void CloseMenu()
    {
        foodCanvas.SetActive(false);
        foodTableUI.UpdateInventory();
        CharacterMovement.Instance.CanOnlyInteract(false);
        CharacterMovement.Instance.CloseMenu();

        if (clothReady)
        {
            clothReady = false;
            ClothManager.Instance.AddCloth(cloth);
            cloth = null;
        }

        menuOpened = false;
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

    private IEnumerator WaitForNewClothing(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foodTableUI.ClothSliderActivate();
    }
}
