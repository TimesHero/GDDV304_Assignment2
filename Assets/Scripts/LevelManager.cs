#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public Tilemap backgroundTilemap;
    public Tilemap foregroundTilemap;
    public Tilemap interactionTilemap;

    public int levelIndex;
    private const string FILE_NAME = "Level";

    public void SaveLevel() 
    { 
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        
        levelData.levelIndex = levelIndex;
        levelData.name = $"{FILE_NAME}_{levelIndex}";

        levelData.backgroundTiles = GetTilesFromTilemap(backgroundTilemap).ToList();
        levelData.foregroundTiles = GetTilesFromTilemap(foregroundTilemap).ToList();
        levelData.interactionTiles = GetTilesFromTilemap(interactionTilemap).ToList();

        ScriptableLevelUtility.SaveLevelAssetFile(levelData);
    }

    private IEnumerable<TileData> GetTilesFromTilemap(Tilemap tilemap)
    {
        foreach(Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                TileData tileData = new ()
                {
                    position = position,
                    tile = tilemap.GetTile<Tile>(position)
                };
                yield return tileData;
            }
        }
    }

    public void LoadLevel() 
    {
        LevelData levelData = Resources.Load<LevelData>($"Levels/{FILE_NAME}_{levelIndex}");

        if (levelData == null)
        {
            Debug.LogError($"Unable to load {FILE_NAME}_{levelIndex}. File not found.");
            return;
        }

        ClearLevel();

        SetTilemapTiles(levelData);
    }

    private void SetTilemapTiles(LevelData levelData)
    {
        foreach(TileData tileData in levelData.backgroundTiles)
            backgroundTilemap.SetTile(tileData.position, tileData.tile);

        foreach (TileData tileData in levelData.foregroundTiles)
            foregroundTilemap.SetTile(tileData.position, tileData.tile);

        foreach (TileData tileData in levelData.interactionTiles)
            interactionTilemap.SetTile(tileData.position, tileData.tile);
    }

    public void ClearLevel() 
    { 
        backgroundTilemap.ClearAllTiles(); 
        foregroundTilemap.ClearAllTiles(); 
        interactionTilemap.ClearAllTiles();
    }
}

public static class ScriptableLevelUtility
{
    public static void SaveLevelAssetFile(LevelData levelData)
    {
        AssetDatabase.CreateAsset(levelData, $"Assets/Resources/Levels/{levelData.name}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif