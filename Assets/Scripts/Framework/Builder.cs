using UnityEngine;

public abstract class Builder : MonoBehaviour
{
    [SerializeField] protected Generator generator;

    protected int[,] grid;
    protected int width;
    protected int height;

    public abstract void Initialize();
    public abstract void GenerateGrid();
    public abstract void ClearGrid();
    public abstract void LoadGrid(int[,] grid);
    public abstract int[,] GetGrid();
}
