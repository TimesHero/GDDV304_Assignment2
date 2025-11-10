using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DiamondSquareGenerator : Generator
{
    private int size;

    [Header("Diamond-Square Settings")]
    public float roughness = 2.0f;

    public override void Initialize(WorldSettings settings)
    {
        size = settings.width > settings.height ? settings.width : settings.height;
        size = Mathf.ClosestPowerOfTwo(size) + 1;

        map = new float[size, size];

        SetInitialCorners();
    }

    private void SetInitialCorners()
    {
        map[0, 0] = Random.value;
        map[0, size - 1] = Random.value;
        map[size - 1, 0] = Random.value;
        map[size - 1, size - 1] = Random.value;
    }

    public override float[,] Generate()
    {
        DiamondSquare(size - 1, roughness);
        return map;
    }

    private void DiamondSquare(int chunkSize, float roughness)
    {
        if (chunkSize <= 1) return; // base case

        SquareStep(chunkSize, roughness);
        DiamondStep(chunkSize, roughness);

        chunkSize /= 2;
        roughness /= 2;

        DiamondSquare(chunkSize, roughness);
    }

    private void SquareStep(int chunkSize, float roughness)
    {
        int halfChunk = chunkSize / 2;

        for (int x = 0; x < size - 1; x += chunkSize)
        {
            for (int y = 0;  y < size - 1; y += chunkSize)
            {
                float noiseValue = GetAverageSquare(x, y, chunkSize) + Random.Range(-roughness, roughness);
                map[x +  halfChunk, y + halfChunk] = Mathf.Clamp01(noiseValue);
            }
        }
    }

    private float GetAverageSquare(int x, int y, int chunkSize)
    {
        float[] corners = new float[]
        {
            map[x, y],
            map[x, y + chunkSize],
            map[x + chunkSize, y],
            map[x + chunkSize, y + chunkSize],
        };

        return corners.Average();
    }

    private void DiamondStep(int chunkSize, float roughness)
    {
        int halfChunk = chunkSize / 2;

        for (int x = 0; x < size - 1; x += halfChunk)
        {
            for (int y = (x + halfChunk) % chunkSize; y < size - 1; y += chunkSize)
            {
                float noiseValue = GetAverageDiamond(x, y, halfChunk) + Random.Range(-roughness, roughness);
                map[x, y] = Mathf.Clamp01(noiseValue);
            }
        }
    }

    private float GetAverageDiamond(int x, int y, int halfChunk)
    {
        List<float> neighbours = new();

        if (x - halfChunk >= 0) neighbours.Add(map[x - halfChunk, y]);
        if (x + halfChunk < size) neighbours.Add(map[x + halfChunk, y]);

        if (y - halfChunk >= 0) neighbours.Add(map[x, y - halfChunk]);
        if (y + halfChunk < size) neighbours.Add(map[x, y + halfChunk]);

        return neighbours.Average();
    }
}
