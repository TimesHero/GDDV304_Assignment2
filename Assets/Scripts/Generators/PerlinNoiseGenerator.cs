using UnityEngine;

public class PerlinNoiseGenerator : Generator
{
    private int width;
    private int height;

    [Header("Perlin Noise Settings")]
    [Range(0.1f, 25f)] public float scale = 5f;
    public bool randomSeed = false;
    [Tooltip("Ignored if random seed is true.")]
    public float noiseSeed = 0f;

    public override void Initialize(WorldSettings settings)
    {
        width = settings.width;
        height = settings.height;

        map = new float[width, height];

        if (randomSeed) noiseSeed = Random.Range(0f, 10000f);
    }

    public override float[,] Generate()
    {
        GenerateNoiseMap();
        return map;
    }

    private void GenerateNoiseMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sampleX = (float)x / width * scale + noiseSeed;
                float sampleY = (float)y / height * scale + noiseSeed;

                float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                map[x, y] = noiseValue;
            }
        }
    }
}
