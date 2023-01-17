using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothManager : MonoBehaviour
{
    [SerializeField] private GameObject clothAnim;
    [SerializeField] private Transform backPackLoc;
    [SerializeField] private AnimationFunctions unlockedText;

    [Header("Hat")]
    public List<Clothing> hatList = new List<Clothing>();

    [Header("Shirt")]
    public List<Clothing> shirtList = new List<Clothing>();

    [Header("Pants")]
    public List<Clothing> pantsList = new List<Clothing>();

    [Header("Shoes")]
    public List<Clothing> shoesList = new List<Clothing>();

    public void AddCloth(Clothing cloth)
    {
        unlockedText.gameObject.SetActive(true);
        unlockedText.PlayAnimation();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Scribble", transform.position);
        Instantiate(clothAnim, transform.position, new Quaternion(0, 0, 0, 0), backPackLoc).GetComponent<GainClothing>().GetClothing(cloth);

        switch (cloth.itemType)
        {
            case Clothing.ClothType.Hat:
                hatList.Add(cloth);
                break;
            case Clothing.ClothType.Shirt:
                shirtList.Add(cloth);
                break;
            case Clothing.ClothType.Pants:
                pantsList.Add(cloth);
                break;
            case Clothing.ClothType.Shoes:
                shoesList.Add(cloth);
                break;
            default:
                break;
        }
    }

    #region Singleton
    private static ClothManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static ClothManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClothManager();
            }

            return instance;
        }
    }
    #endregion
}
