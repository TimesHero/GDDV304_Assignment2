using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TowerType
{
    Arrow = 0,
    Mage = 1
}
public class InteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CursorController cursor;
    [Header("UI")]
    [SerializeField] private Transform panelPivot;
    [SerializeField] private Animator panelAnimator;
    [Header("Placeable Units")]
    [SerializeField] private Tilemap unitTilemap;
    [SerializeField] private UnitTile[] unitTiles;

    private bool isOpen = false;

    private readonly int popInState = Animator.StringToHash("PopIn");
    private readonly int popOutState = Animator.StringToHash("PopOut");

    private Vector2 openPosition;
    private Vector3Int spawnPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            if (cursor.HitInfo)
            {
                openPosition = cursor.TileScreenPosition;
                spawnPosition = cursor.MouseTilemapPosition;
                StartCoroutine(OpenPanel());
            }
            else if (isOpen)
            {
                ClosePanel();
            }
        }

    }

    public void Close()
    {
        StartCoroutine(ClosePanel());
    }

    private IEnumerator OpenPanel()
    {
        if (isOpen) yield return ClosePanel();

        panelPivot.gameObject.SetActive(true);
        panelPivot.position = openPosition;

        panelAnimator.Play(popInState);
        isOpen = true;
    }

    private IEnumerator ClosePanel()
    {
        panelAnimator.Play(popOutState);
        float animationDuration = panelAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationDuration);

        panelPivot.gameObject.SetActive(false);
        isOpen = false;
    }

    public void PlaceTower(string whichTower)
    {
        TowerType which = System.Enum.Parse<TowerType>(whichTower);

        switch(which)
        {
            case TowerType.Arrow:
                unitTilemap.SetTile(spawnPosition, unitTiles[0]);
                break;
            case TowerType.Mage:
                unitTilemap.SetTile(spawnPosition, unitTiles[1]);
                break;
        }
    }
}
