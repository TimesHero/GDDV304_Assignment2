using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBuilder : Builder
{
    public enum BiomeType
    { 
        Water,
        Sand,
        Grass,
        DarkGrass
    }

    public WorldSettings settings;
    public Tilemap backgroundTilemap;
    [SerializeField] private Tilemap foregroundTilemap;
    [SerializeField] private Tilemap interactionTilemap;

    public override void Initialize()
    {
        ClearGrid();
        generator.Initialize(settings);
    }

    public override void ClearGrid()
    {
        StopAllCoroutines();
        backgroundTilemap.ClearAllTiles();
    }

    [ContextMenu("Generate Grid")]
    public override void GenerateGrid()
    {
        Initialize();

        float[,] noiseMap = generator.Generate();
        StartCoroutine(FillTileGrid(noiseMap));
    }

    private IEnumerator FillTileGrid(float[, ] noiseMap)
    {
        width = noiseMap.GetLength(0);
        height = noiseMap.GetLength(1);

        SetTiles(noiseMap);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                AddTile(x, y, grid[x, y]);
            }
            yield return null;
        }
    }

    private void SetTiles(float[,] noiseMap)
    {
        grid = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = DetermineBiomeTile(noiseMap[x, y]);
            }
        }
    }

    private int DetermineBiomeTile(float noiseSample)
    {
        for (int i = 0; i < settings.biomes.Length; i++)
            if (noiseSample <= settings.biomes[i].BiomeThreshold) return i;
        
        return settings.biomes.Length - 1;
    }

    private void AddTile(int x, int y, int biome)
    {
        Vector3Int tilePos = new(x, y, 0);
        TileBase tile = settings.tiles[biome];

        backgroundTilemap.SetTile(tilePos, settings.tiles[biome]);

        if (foregroundTilemap != null)
        {
            foregroundTilemap.SetTile(tilePos, tile);
        }
        if (interactionTilemap != null)
        {
            interactionTilemap.SetTile(tilePos, tile);
        }

    }

    public override void LoadGrid(int[,] grid)
    {
    }

    public override int[,] GetGrid()
    {
        return grid;
    }

    private void OnGUI()
    {
        GUIStyle buttonStyle = GUI.skin.button;

        buttonStyle.fontSize = 24;

        if (GUI.Button(new Rect(10, 10, 320, 80), "Generate!", buttonStyle)) 
        {
            GenerateGrid();
        }
    }
}
