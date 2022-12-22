using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish")]
public class FishStats : ScriptableObject
{
    public Item item;
    public ItemPickup.ItemType itemType;
    public Color32 color;
    [Header("Difficulty")]
    public float innerDisVisualCircle;
    public float maxDisVisualCircle;
    public float maxDisToTargetDot;
    public float closeToTargetRadius;
    public float minSizePlayerDot;
    public float maxSizePlayerDot;
    public float sizeInInnerCircle;
}
