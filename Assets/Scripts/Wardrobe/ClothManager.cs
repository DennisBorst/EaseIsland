using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothManager : MonoBehaviour
{
    //public enum ChangeObject
    //{
    //    Hair,
    //    Skin,
    //    Hat,
    //    Shirt,
    //    Pants,
    //    Shoes
    //}

    //private ChangeObject currentObject;
    //private CharacterDress characterDress;
    //[SerializeField] private SkinnedMeshRenderer characterMeshRen;
    //[SerializeField] private int[] accesioreCountList;

    //[Header("Hair")]
    //[SerializeField] private Material[] hairColor;

    //[SerializeField] private Material[] skinColor;

    [SerializeField] private GameObject clothAnim;
    [SerializeField] private Transform backPackLoc;
    [SerializeField] private GameObject unlockedText;

    [Header("Hat")]
    public List<Clothing> hatList = new List<Clothing>();
    //[SerializeField] private Transform hat;

    [Header("Shirt")]
    public List<Clothing> shirtList = new List<Clothing>();
    //[SerializeField] private Transform shirtBody;
    //[SerializeField] private Transform shirtPipeL;
    //[SerializeField] private Transform shirtPipeR;

    [Header("Pants")]
    public List<Clothing> pantsList = new List<Clothing>();
    //[SerializeField] private Transform pipeL;
    //[SerializeField] private Transform pipeR;

    [Header("Shoes")]
    public List<Clothing> shoesList = new List<Clothing>();
    //[SerializeField] private Transform shoeL;
    //[SerializeField] private Transform shoeR;

    //[SerializeField] private List<ClothChangeButton> buttons = new List<ClothChangeButton>();


    public void AddCloth(Clothing cloth)
    {
        unlockedText.SetActive(true);
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

    //private GameObject lastButton;
    //private int changeCount;
    //private int accessoireCount;

    //public void AddCloth(Clothing cloth)
    //{
    //    switch (cloth.itemType)
    //    {
    //        case Clothing.ClothType.Hat:
    //            hatList.Add(cloth);
    //            break;
    //        case Clothing.ClothType.Shirt:
    //            shirtList.Add(cloth);
    //            break;
    //        case Clothing.ClothType.Pants:
    //            pantsList.Add(cloth);
    //            break;
    //        case Clothing.ClothType.Shoes:
    //            shoesList.Add(cloth);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //public void LoadInStats()
    //{
    //    changeCount = 0;
    //    currentObject = (ChangeObject)changeCount;
    //    UpdateButtons();
    //}

    //public void UpdateStats()
    //{
    //    characterDress.UpdateCharacter(
    //        characterMeshRen,
    //        hatList[accesioreCountList[2]],
    //        shirtList[accesioreCountList[3]],
    //        pantsList[accesioreCountList[4]],
    //        shoesList[accesioreCountList[5]]);
    //}

    //public void ClothSelected(ClothChangeButton buttonScript, GameObject button)
    //{
    //    lastButton = button;
    //    changeCount = buttons.IndexOf(buttonScript);
    //    currentObject = (ChangeObject)changeCount;
    //    //UpdateCloth();
    //}

    //public void SwitchAccessoire(int direction)
    //{
    //    Vector2 accessoireNumber = GetIntAccessoire();
    //    accessoireCount = (int)accessoireNumber.x;
    //    accessoireCount += direction;
    //    if (accessoireCount < 0) { accessoireCount = (int)accessoireNumber.y - 1; }
    //    else if (accessoireCount >= (int)accessoireNumber.y) { accessoireCount = 0; }

    //    UpdateUI();
    //}

    //private void UpdateButtons()
    //{
    //    buttons[0].UpdateUI(hairColor[accesioreCountList[0]].name);
    //    buttons[1].UpdateUI(skinColor[accesioreCountList[1]].name);
    //    buttons[2].UpdateUI(hatList[accesioreCountList[2]].objectName);
    //    buttons[3].UpdateUI(shirtList[accesioreCountList[3]].objectName);
    //    buttons[4].UpdateUI(pantsList[accesioreCountList[4]].objectName);
    //    buttons[5].UpdateUI(shoesList[accesioreCountList[5]].objectName);

    //    //buttons[2].UpdateUI();
    //    //buttons[3].UpdateUI();
    //    //buttons[4].UpdateUI();
    //    //buttons[5].UpdateUI();
    //}

    //private void UpdateUI()
    //{
    //    switch (currentObject)
    //    {
    //        case ChangeObject.Hair:
    //            accesioreCountList[0] = accessoireCount;
    //            buttons[changeCount].UpdateUI(hairColor[accesioreCountList[0]].name);
    //            Material[] matsHair = characterMeshRen.materials;
    //            matsHair[2] = hairColor[accesioreCountList[0]];
    //            characterMeshRen.materials = matsHair;
    //            break;
    //        case ChangeObject.Skin:
    //            accesioreCountList[1] = accessoireCount;
    //            buttons[changeCount].UpdateUI(skinColor[accesioreCountList[1]].name);
    //            Material[] matsSkin = characterMeshRen.materials;
    //            matsSkin[0] = skinColor[accesioreCountList[1]];
    //            characterMeshRen.materials = matsSkin;
    //            break;
    //        case ChangeObject.Hat:
    //            accesioreCountList[2] = accessoireCount;
    //            buttons[changeCount].UpdateUI(hatList[accesioreCountList[changeCount]].objectName);
    //            Destroy(hat.transform.GetChild(0).gameObject);
    //            Instantiate(hatList[accesioreCountList[changeCount]].hat, this.hat.position, this.hat.rotation, this.hat).layer = 0;
    //            break;
    //        case ChangeObject.Shirt:
    //            accesioreCountList[3] = accessoireCount;
    //            buttons[changeCount].UpdateUI(shirtList[accesioreCountList[changeCount]].objectName);
    //            Destroy(shirtBody.transform.GetChild(0).gameObject);
    //            Destroy(shirtPipeL.transform.GetChild(0).gameObject);
    //            Destroy(shirtPipeR.transform.GetChild(0).gameObject);
    //            Instantiate(shirtList[accesioreCountList[changeCount]].shirt[0], this.shirtBody.position, this.shirtBody.rotation, this.shirtBody).layer = 0;
    //            Instantiate(shirtList[accesioreCountList[changeCount]].shirt[1], this.shirtPipeL.position, this.shirtPipeL.rotation, this.shirtPipeL).layer = 0;
    //            Instantiate(shirtList[accesioreCountList[changeCount]].shirt[2], this.shirtPipeR.position, this.shirtPipeR.rotation, this.shirtPipeR).layer = 0;
    //            break;
    //        case ChangeObject.Pants:
    //            accesioreCountList[4] = accessoireCount;
    //            buttons[changeCount].UpdateUI(pantsList[accesioreCountList[4]].objectName);
    //            Destroy(pipeL.transform.GetChild(0).gameObject);
    //            Destroy(pipeR.transform.GetChild(0).gameObject);
    //            Instantiate(pantsList[accesioreCountList[changeCount]].pants[0], this.pipeL.position, this.pipeL.rotation, this.pipeL).layer = 0;
    //            Instantiate(pantsList[accesioreCountList[changeCount]].pants[1], this.pipeR.position, this.pipeR.rotation, this.pipeR).layer = 0;
    //            break;
    //        case ChangeObject.Shoes:
    //            accesioreCountList[5] = accessoireCount;
    //            buttons[changeCount].UpdateUI(shoesList[accesioreCountList[changeCount]].objectName);
    //            Destroy(shoeL.transform.GetChild(0).gameObject);
    //            Destroy(shoeR.transform.GetChild(0).gameObject);
    //            Instantiate(shoesList[accesioreCountList[changeCount]].shoes[0], this.shoeL.position, this.shoeL.rotation, this.shoeL).layer = 0;
    //            Instantiate(shoesList[accesioreCountList[changeCount]].shoes[1], this.shoeR.position, this.shoeR.rotation, this.shoeR).layer = 0;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //private Vector2 GetIntAccessoire()
    //{
    //    int number = 0;
    //    int panelAmount = 0;

    //    switch (currentObject)
    //    {
    //        case ChangeObject.Hair:
    //            number = accesioreCountList[0];
    //            panelAmount = hairColor.Length;
    //            break;
    //        case ChangeObject.Skin:
    //            number = accesioreCountList[1];
    //            panelAmount = skinColor.Length;
    //            break;
    //        case ChangeObject.Hat:
    //            number = accesioreCountList[2];
    //            panelAmount = hatList.Count;
    //            break;
    //        case ChangeObject.Shirt:
    //            number = accesioreCountList[3];
    //            panelAmount = shirtList.Count;
    //            break;
    //        case ChangeObject.Pants:
    //            number = accesioreCountList[4];
    //            panelAmount = pantsList.Count;
    //            break;
    //        case ChangeObject.Shoes:
    //            number = accesioreCountList[5];
    //            panelAmount = shoesList.Count;
    //            break;
    //        default:
    //            break;
    //    }

    //    return new Vector2(number, panelAmount);
    //}

    //private void Start()
    //{
    //    characterDress = CharacterMovement.Instance.GetComponent<CharacterDress>();
    //}

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
