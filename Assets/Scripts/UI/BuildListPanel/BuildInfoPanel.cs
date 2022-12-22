using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInfoPanel : MonoBehaviour
{
    [SerializeField] private BuildObjectInfo objectInfo;
    [SerializeField] private ItemAmountUI itemAmountInfo;
    [SerializeField] private GameObject buildObjectButton;
    [SerializeField] private GameObject buildDiscoveredText;
    [SerializeField] private Transform objectLoc;
    [Space]
    [SerializeField] private Building emptyInfo;
    [SerializeField] private Building[] startInfo;
    [SerializeField] private List<Building> discoveredBuildings = new List<Building>();

    private int buildInfoCount = 0;
    private SelectUIObject selectUIObject;
    private Building lastBuilding;
    private GameObject lastSelectedButton;

    public void AddBuildingInfo(Building building)
    {
        if (discoveredBuildings.Contains(building)) { return; }


        buildDiscoveredText.SetActive(true);
        discoveredBuildings.Add(building);
        buildInfoCount += 1;

        GameObject newObj = Instantiate(buildObjectButton, objectLoc.transform.position, objectLoc.transform.rotation, objectLoc);
        BuildInfoButton newBuildObj = newObj.GetComponent<BuildInfoButton>();
        newBuildObj.buildInfoPanel = this;
        newBuildObj.buildObject = building;
        newBuildObj.UpdateUI();

        if(buildInfoCount == 1)
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
            AddBuildingInfo(startInfo[i]);
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
