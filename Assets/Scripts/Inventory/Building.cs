using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public string buildingName;
    public GameObject buildObject;

    [Serializable]
    public struct Necessities
    {
        public Item item;
        public int amount;
    }
    public Necessities[] necessities;
}