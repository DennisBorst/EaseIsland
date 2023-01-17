using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [Range(0f, 100f)]
    [SerializeField] private float startAmount;
    [SerializeField] private float decreaseHungerOnTick = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float timer = 0.2f;

    private List<FoodTableUI> foodTables = new List<FoodTableUI>();
    private List<Slider> sliders = new List<Slider>();
    private float hunger;

    public void IncreaseFood(float amount)
    {
        hunger += amount;

        if(hunger > 100f)
        {
            hunger = 100f;
        }
    }

    public void StartFoodTimer()
    {
        StartCoroutine(Timer());
    }

    public void AddFoodTable(FoodTableUI table)
    {
        foodTables.Add(table);
        Debug.Log(foodTables.Count);
    }

    public void AddSlider(Slider slider)
    {
        sliders.Add(slider);
    }

    public bool FoodStatus()
    {
        if (hunger > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        hunger = startAmount;
    }

    private IEnumerator Timer()
    {
        WaitForSeconds wait = new WaitForSeconds(timer);

        while (true)
        {
            if (GameManger.Instance.gameIsFrozen) { yield return wait; }
            yield return wait;
            DecreaseHunger();
        }
    }

    private void DecreaseHunger()
    {
        hunger -= decreaseHungerOnTick;

        if(hunger <= 0)
        {
            hunger = 0f;
            return;
        }

        if(sliders.Count != 0) { sliders[0].value = hunger; }

        if(foodTables.Count == 0) { return; }
        foodTables[0].UpdateSlider(hunger);
    }

    #region Singleton
    private static FoodManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static FoodManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FoodManager();
            }

            return instance;
        }
    }
    #endregion
}
