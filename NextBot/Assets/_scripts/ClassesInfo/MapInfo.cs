using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Map Info", menuName = "Map Info")]
public class MapInfo:ScriptableObject
{
    public int MapIndex;
    public string Name;
    public Sprite Image;


}
