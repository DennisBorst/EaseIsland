using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInfoPanel : MonoBehaviour
{
    [SerializeField] private BuildObjectInfo objectInfo;
    [SerializeField] private ItemAmountUI itemAmountInfo;
    [SerializeField] private GameObject buildObjectButton;
    [SerializeField] private Transform objectLoc;
    [Space]
    [SerializeField] private Building[] startInfo;

    private int buildInfoCount = 0;
    private SelectUIObject selectUIObject;
    private Building lastBuilding;

    public void AddBuildingInfo(Building building)
    {
        buildInfoCount += 1;

        GameObject newObj = Instantiate(buildObjectButton, objectLoc.transform.position, objectLoc.transform.rotation, objectLoc);
        BuildInfoButton newBuildObj = newObj.GetComponent<BuildInfoButton>();
        newBuildObj.buildInfoPanel = this;
        newBuildObj.buildObject = building;
        newBuildObj.UpdateUI();

        if(buildInfoCount == 1)
        {
            selectUIObject.selectAsFirstUI = newObj;
            selectUIObject.SelectFirstUIElemnt();
        }

    }

    public void UpdateUI()
    {
        itemAmountInfo.UpdateItems();
        objectInfo.UpdateBuildItem(lastBuilding);
    }

    public void BuildInfoSelected(Building building)
    {
        lastBuilding = building;
        objectInfo.UpdateBuildItem(building);
    }

    private void Start()
    {
        selectUIObject = GetComponent<SelectUIObject>();

        for (int i = 0; i < startInfo.Length; i++)
        {
            AddBuildingInfo(startInfo[i]);
        }
    }

    private void OnEnable()
    {
        if(lastBuilding == null) { return; }
        UpdateUI();
    }
}
