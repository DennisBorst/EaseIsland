using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Normal,
        Food,
        Tools,
        Crystal
    }

    public string ItemName;
    [TextArea(10, 20)]
    public string itemDescription;
    public ItemPickup.ItemType item;
    public ItemType itemType;
    public int maxStack;
    public int animNumber;
    public Sprite inventoryImg;
    public GameObject prefabItem;
    [Space]
    public float foodAmount;
}
