using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clothing", menuName = "Clothing")]
public class Clothing : ScriptableObject
{
    public string objectName;

    public enum ClothType
    {
        Hat,
        Shirt,
        Pants,
        Shoes
    }

    public ClothType itemType;

    public GameObject hat;
    public GameObject[] shirt;
    public GameObject[] pants;
    public GameObject[] shoes;

    public GameObject itemObject;
}
