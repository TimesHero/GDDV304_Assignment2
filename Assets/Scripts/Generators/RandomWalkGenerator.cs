using UnityEngine;

public class RandomWalkGenerator : Generator
{
    [Header("Random Walk Settings")]
    public int walkLength = 2500;
    [Range(0, 1f)]
    public float walkStraightProbability = 0.45f;
    public bool allowBacktracking = true;

    public bool randomSeed = false;
    [Tooltip("Ignored if random seed is true.")]
    public int seed = 12345;

    private int width;
    private int height;
    private Vector2Int currentDirection;

    private const float WALL = 1f;
    private const float FLOOR = 0f;

    private static readonly Vector2Int[] Directions = new[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public override void Initialize(WorldSettings settings)
    {
        if (!randomSeed) Random.InitState(seed);
        
        width = settings.width;
        height = settings.height;

        map = new float[width, height];

        FillWithWalls();
    }

    private void FillWithWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = WALL;
            }
        }
    }

    public override float[,] Generate()
    {
        Vector2Int currentPosition = new (width / 2, height / 2);

        for (int step = 0; step < walkLength; step++)
        {
            map[currentPosition.x, currentPosition.y] = FLOOR;
            currentPosition = TakeStep(currentPosition);
        }

        return map;
    }

    private Vector2Int TakeStep(Vector2Int currentPosition)
    {
        if (Random.value > walkStraightProbability) 
            currentDirection = PickRandomDirection();
        
        Vector2Int newPosition = currentPosition + currentDirection;
        newPosition = ClampPositionToMap(newPosition);

        return newPosition;
    }

    private Vector2Int PickRandomDirection()
    {
        Vector2Int newDirection;

        int attempts = 0;
        const int MAX_ATTEMPTS = 10;

        do
        {
            newDirection = Directions[Random.Range(0, Directions.Length)];
            attempts++;
        } while(!allowBacktracking && newDirection == -currentDirection && attempts < MAX_ATTEMPTS);

        return newDirection;
    }

    private Vector2Int ClampPositionToMap(Vector2Int position)
    {
        position.x = Mathf.Clamp(position.x, 0, width - 1);
        position.y = Mathf.Clamp(position.y, 0, height - 1);

        return position;
    }
}
