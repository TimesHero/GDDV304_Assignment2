using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    protected float[,] map;

    public abstract void Initialize(WorldSettings settings);
    public abstract float[, ] Generate( );
}