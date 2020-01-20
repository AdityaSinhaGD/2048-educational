using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStyleController : Singleton<TileStyleController>
{
    public TileStyle[] tileStyles;
    
}

[System.Serializable]
public class TileStyle
{
    public int number;
    public Color tileColor;
    public Color textColor;
}