using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera viewCamera;
    [Header("Interaction Settings")]
    [SerializeField] private Tilemap interactionMap;
    [SerializeField] private LayerMask interactionMask;
    [Header("Graphics Settings")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Animator animator;

    public Vector2 MouseWorldPosition { get; private set; } = Vector2.zero;
    public Vector3Int MouseTilemapPosition { get; private set; } = Vector3Int.zero;
    public Vector2 TileScreenPosition { get; private set; } = Vector2.zero;
    public RaycastHit2D HitInfo { get; private set; } = new RaycastHit2D();

    private readonly int IsPulsingParam = Animator.StringToHash("IsPulsing");

    private void Start()
    {
        Debug.Log(IsPulsingParam);
    }

    private void Update()
    {
        CheckCursorTilemapPosition();
    }

    private void CheckCursorTilemapPosition()
    {
        cursorImage.enabled = !EventSystem.current.IsPointerOverGameObject();
        if (!cursorImage.enabled) return;

        Vector2 mouseScreenPos = Input.mousePosition;
        MouseWorldPosition = viewCamera.ScreenToWorldPoint(mouseScreenPos);
        cursorImage.rectTransform.position = mouseScreenPos;

        HitInfo = Physics2D.Raycast(MouseWorldPosition, Vector3.forward, float.MaxValue, interactionMask);
        if (HitInfo)
        {
            MouseTilemapPosition = interactionMap.WorldToCell(HitInfo.point);
            TileScreenPosition = viewCamera.WorldToScreenPoint(MouseTilemapPosition);

            cursorImage.rectTransform.position = TileScreenPosition + cursorImage.rectTransform.sizeDelta / 2;

            if (animator) animator.SetBool(IsPulsingParam, true);
            cursorImage.color = Color.cyan;
        }
        else
        {
            if (animator) animator.SetBool(IsPulsingParam, false);
            cursorImage.color = new Color(1f, 0.5f, 0f);
        }
    }
}
