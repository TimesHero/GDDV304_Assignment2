using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractionLayerSetup : MonoBehaviour
{
    public bool forceReconfigure = true;
    public CompositeCollider2D.GeometryType geometryType = CompositeCollider2D.GeometryType.Polygons;
    public CompositeCollider2D.GenerationType generationType = CompositeCollider2D.GenerationType.Synchronous;

    void Awake()
    {
        EnsureComponents();
    }

    private void EnsureComponents()
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        if (forceReconfigure || rb.bodyType != RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        
        var tmc = GetComponent<TilemapCollider2D>();
        if (tmc == null)
        {
            tmc = gameObject.AddComponent<TilemapCollider2D>();
        }
        if (forceReconfigure || !tmc.usedByComposite)
        {
            tmc.usedByComposite = true;
        }

        var comp = GetComponent<CompositeCollider2D>();
        if (comp == null)
        {
            comp = gameObject.AddComponent<CompositeCollider2D>();
        }
        if (forceReconfigure)
        {
            comp.geometryType = geometryType;              
            comp.generationType = generationType;          
        }
    }

    public void RefreshColliders()
    {
        var tmc = GetComponent<TilemapCollider2D>();
        if (tmc) tmc.ProcessTilemapChanges();

        var comp = GetComponent<CompositeCollider2D>();
        if (comp) comp.GenerateGeometry();
    }
}
