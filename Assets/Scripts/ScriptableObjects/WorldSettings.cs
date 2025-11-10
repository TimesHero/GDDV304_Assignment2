using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct BiomeSettings
{
    public string BiomeName;
    public float BiomeThreshold;
}

[CreateAssetMenu(fileName = "WorldSettings", menuName = "Scriptable Objects/World Settings...")]
public class WorldSettings : ScriptableObject
{
    [Range(3, 256)] public int width = 64;
    [Range(3, 256)] public int height = 64;
    public BiomeSettings[] biomes;
    public TileBase[] tiles;
}
