using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private float decreaseHungerOnTick = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float timer = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float hungerWalkSpeed = 0.8f;
    [Space]
    [SerializeField] private Slider foodSlider;

    private CharacterMovement characterMovement;
    private float hunger;

    public void IncreaseFood(float amount)
    {
        hunger += amount;

        if(hunger > 100f)
        {
            hunger = 100f;
        }
    }

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        StartCoroutine(Timer());
        hunger = 100f;
        foodSlider.maxValue = hunger - 10f;
        foodSlider.value = hunger;
    }

    private IEnumerator Timer()
    {
        WaitForSeconds wait = new WaitForSeconds(timer);

        while (true)
        {
            yield return wait;
            DecreaseHunger();
        }
    }

    private void DecreaseHunger()
    {
        hunger -= decreaseHungerOnTick;
        foodSlider.value = hunger;

        if(hunger <= 0)
        {
            hunger = 0f;
            characterMovement.DecreaseMovementSpeed(hungerWalkSpeed);
            return;
        }

        characterMovement.DecreaseMovementSpeed(1f);
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
