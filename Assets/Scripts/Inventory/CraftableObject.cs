using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New CraftableObject", menuName = "CraftableObject")]
public class CraftableObject : ScriptableObject
{
    public Item craftableItem;

    [Serializable]
    public struct Necessities
    {
        public Item item;
        public int amount;
    }
    public Necessities[] necessities;
}
