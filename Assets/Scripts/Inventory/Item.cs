using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Normal,
        Food,
        Tools
    }

    public string ItemName;
    public ItemPickup.ItemType item;
    public ItemType itemType;
    public int maxStack;
    public int animNumber;
    public Sprite inventoryImg;
    public GameObject prefabItem;
    [Space]
    public float foodAmount;
}
