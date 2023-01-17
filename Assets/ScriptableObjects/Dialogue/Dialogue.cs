using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string name;
    [TextArea(3,5)]
    public string[] sentences;
    public string[] optionNames;
}
