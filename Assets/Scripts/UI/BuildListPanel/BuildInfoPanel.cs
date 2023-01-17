using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInfoPanel : MonoBehaviour
{
    [SerializeField] private BuildObjectInfo objectInfo;
    [SerializeField] private ItemAmountUI itemAmountInfo;
    [SerializeField] private GameObject buildObjectButton;
    [SerializeField] private AnimationFunctions buildDiscoveredText;
    [SerializeField] private Transform objectLoc;
    [Space]
    [SerializeField] private Building emptyInfo;
    [SerializeField] private Building[] startInfo;
    [SerializeField] private List<Building> discoveredBuildings = new List<Building>();
    [SerializeField] private List<BuildInfoButton> buildButtons = new List<BuildInfoButton>();

    private int buildInfoCount = 0;
    [SerializeField] private SelectUIObject selectUIObject;
    private Building lastBuilding;
    private GameObject lastSelectedButton;

    public void AddBuildingInfo(Building building, bool startInfo = false)
    {
        if (discoveredBuildings.Contains(building)) { return; }
        if (!startInfo) 
        {
            buildDiscoveredText.gameObject.SetActive(true);
            buildDiscoveredText.PlayAnimation();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Scribble", transform.position);
        }
        
        discoveredBuildings.Add(building);
        buildInfoCount += 1;

        GameObject newObj = Instantiate(buildObjectButton, objectLoc.transform.position, objectLoc.transform.rotation, objectLoc);
        BuildInfoButton newBuildObj = newObj.GetComponent<BuildInfoButton>();
        newBuildObj.buildInfoPanel = this;
        newBuildObj.buildObject = building;
        newBuildObj.UpdateUI();

        buildButtons.Add(newBuildObj);

        if (buildInfoCount == 1)
        {
            selectUIObject.selectAsFirstUI = newObj;
            lastBuilding = building;
            selectUIObject.SelectFirstUIElement();
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        itemAmountInfo.UpdateItems();

        if (lastBuilding == null) { objectInfo.UpdateBuildItem(emptyInfo); }
        else { objectInfo.UpdateBuildItem(lastBuilding); }

        for (int i = 0; i < buildButtons.Count; i++)
        {
            buildButtons[i].UpdateUI();
        }
    }

    public void BuildInfoSelected(Building building, GameObject button)
    {
        lastBuilding = building;
        objectInfo.UpdateBuildItem(building);
        lastSelectedButton = button;
    }

    private void Start()
    {
        selectUIObject = GetComponent<SelectUIObject>();
        UpdateUI();

        for (int i = 0; i < startInfo.Length; i++)
        {
            AddBuildingInfo(startInfo[i], true);
        }
    }

    private void OnEnable()
    {
        if(lastBuilding == null) { return; }
        UpdateUI();

        if (lastSelectedButton != null)
        {
            selectUIObject.selectAsFirstUI = lastSelectedButton;
            selectUIObject.SelectFirstUIElement();
            return;
        }
    }

    #region Singleton
    private static BuildInfoPanel instance;
    private void Awake()
    {
        instance = this;
    }
    public static BuildInfoPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuildInfoPanel();
            }

            return instance;
        }
    }
    #endregion
}
