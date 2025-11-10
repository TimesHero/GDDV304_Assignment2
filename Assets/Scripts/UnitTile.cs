using UnityEngine.Tilemaps;
using UnityEngine;

public enum UnitType
{
    Tower = 0,
    Mob = 1
}

[CreateAssetMenu(fileName = "New Unit Tile", menuName = "2D/Tiles/Unit Tile")]

public class UnitTile : Tile
{
    public UnitType type;
}
