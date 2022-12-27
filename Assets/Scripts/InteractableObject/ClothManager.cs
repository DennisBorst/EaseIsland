using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothManager : MonoBehaviour
{
    public enum ChangeObject
    {
        Hair,
        Skin,
        Hat,
        Shirt,
        Pants,
        Shoes
    }

    private ChangeObject currentObject;
    private CharacterDress characterDress;
    [SerializeField] private SkinnedMeshRenderer characterMeshRen;
    [SerializeField] private int[] accesioreCountList;

    [Header("Hair")]
    [SerializeField] private Material[] hairColor;
   

    [SerializeField] private Material[] skinColor;

    [SerializeField] private List<Clothing> hatList = new List<Clothing>();
    [SerializeField] private List<Clothing> shirtList = new List<Clothing>();
    [SerializeField] private List<Clothing> pantsList = new List<Clothing>();
    [SerializeField] private List<Clothing> shoesList = new List<Clothing>();

    [SerializeField] private List<ClothChangeButton> buttons = new List<ClothChangeButton>();
    
    private GameObject lastButton;
    private int changeCount;
    private int accessoireCount;

    public void LoadInStats()
    {
        UpdateButtons();
    }

    public void UpdateStats()
    {
        characterDress.UpdateCharacter(characterMeshRen);
    }

    public void ClothSelected(ClothChangeButton buttonScript, GameObject button)
    {
        lastButton = button;
        changeCount = buttons.IndexOf(buttonScript);
        currentObject = (ChangeObject)changeCount;
        //UpdateCloth();
    }

    public void SwitchAccessoire(int direction)
    {
        Vector2 accessoireNumber = GetIntAccessoire();
        accessoireCount = (int)accessoireNumber.x;
        accessoireCount += direction;
        if (accessoireCount < 0) { accessoireCount = (int)accessoireNumber.y - 1; }
        else if (accessoireCount >= (int)accessoireNumber.y) { accessoireCount = 0; }

        UpdateUI();
    }

    private void UpdateButtons()
    {
        buttons[0].UpdateUI(hairColor[accesioreCountList[0]].name);
        buttons[1].UpdateUI(skinColor[accesioreCountList[0]].name);
        //buttons[2].UpdateUI();
        //buttons[3].UpdateUI();
        //buttons[4].UpdateUI();
        //buttons[5].UpdateUI();
    }

    private void UpdateUI()
    {
        switch (currentObject)
        {
            case ChangeObject.Hair:
                accesioreCountList[0] = accessoireCount;
                buttons[changeCount].UpdateUI(hairColor[accesioreCountList[0]].name);
                Material[] matsHair = characterMeshRen.materials;
                matsHair[2] = hairColor[accesioreCountList[0]];
                characterMeshRen.materials = matsHair;
                break;
            case ChangeObject.Skin:
                accesioreCountList[1] = accessoireCount;
                buttons[changeCount].UpdateUI(skinColor[accesioreCountList[1]].name);
                Material[] matsSkin = characterMeshRen.materials;
                matsSkin[0] = skinColor[accesioreCountList[1]];
                characterMeshRen.materials = matsSkin;
                break;
            case ChangeObject.Hat:
                accesioreCountList[2] = accessoireCount;

                break;
            case ChangeObject.Shirt:
                accesioreCountList[3] = accessoireCount;

                break;
            case ChangeObject.Pants:
                accesioreCountList[4] = accessoireCount;

                break;
            case ChangeObject.Shoes:
                accesioreCountList[5] = accessoireCount;

                break;
            default:
                break;
        }
    }

    private Vector2 GetIntAccessoire()
    {
        int number = 0;
        int panelAmount = 0;

        switch (currentObject)
        {
            case ChangeObject.Hair:
                number = accesioreCountList[0];
                panelAmount = hairColor.Length;
                break;
            case ChangeObject.Skin:
                number = accesioreCountList[1];
                panelAmount = skinColor.Length;
                break;
            case ChangeObject.Hat:
                number = accesioreCountList[2];
                panelAmount = hatList.Count;
                break;
            case ChangeObject.Shirt:
                number = accesioreCountList[3];
                panelAmount = shirtList.Count;
                break;
            case ChangeObject.Pants:
                number = accesioreCountList[4];
                panelAmount = pantsList.Count;
                break;
            case ChangeObject.Shoes:
                number = accesioreCountList[5];
                panelAmount = shoesList.Count;
                break;
            default:
                break;
        }

        return new Vector2(number, panelAmount);
    }

    private void Start()
    {
        characterDress = CharacterMovement.Instance.GetComponent<CharacterDress>();
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
