using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] private CraftObjectInfo objectInfo;
    [SerializeField] private ItemAmountUI itemAmountInfo;
    [SerializeField] private GameObject toolObjectButton;
    [SerializeField] private Transform objectLoc;
    [Space]
    [SerializeField] private CraftableObject[] startTools;

    private int toolCount = 0;
    private SelectUIObject selectUIObject;
    private CraftableObject lastTool;
    private GameObject lastButton;

    public void AddTool(CraftableObject tool)
    {
        toolCount += 1;

        GameObject newObj = Instantiate(toolObjectButton, objectLoc.transform.position, objectLoc.transform.rotation, objectLoc);
        ToolButton newTool = newObj.GetComponent<ToolButton>();
        newTool.craftingPanel = this;
        newTool.craftObject = tool;
        newTool.UpdateUI();

        if (toolCount == 1)
        {
            selectUIObject.selectAsFirstUI = newObj;
            selectUIObject.SelectFirstUIElemnt();
        }

    }

    public void UpdateUI()
    {
        itemAmountInfo.UpdateItems();
        objectInfo.UpdateCraftItem(lastTool);
    }

    public void ToolSelected(CraftableObject tool, GameObject button)
    {
        lastTool = tool;
        lastButton = button;
        objectInfo.UpdateCraftItem(tool);
    }

    public void CraftItem()
    {
        InventoryManager.Instance.ItemCrafted(lastTool);
        InventoryManager.Instance.AddToInv(lastTool.craftableItem);
        UpdateUI();

        selectUIObject.selectAsFirstUI = lastButton;
        selectUIObject.SelectFirstUIElemnt();
    }

    private void Start()
    {
        selectUIObject = GetComponent<SelectUIObject>();

        for (int i = 0; i < startTools.Length; i++)
        {
            AddTool(startTools[i]);
        }
    }

    private void OnEnable()
    {
        if(lastTool == null) { return; }
        UpdateUI();
    }
}

