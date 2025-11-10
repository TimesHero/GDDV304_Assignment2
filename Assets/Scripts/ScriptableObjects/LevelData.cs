using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public int levelIndex;
    public List<TileData> backgroundTiles;
    public List<TileData> foregroundTiles;
    public List<TileData> interactionTiles;
}