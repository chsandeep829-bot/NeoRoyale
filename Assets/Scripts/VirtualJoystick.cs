using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Add to a UI joystick background. The RectTransform center is the neutral point.
/// </summary>
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform handle;
    [Range(0.2f, 1f)] public float handleRange = 0.65f;

    private RectTransform area;
    private Vector2 startPosition;

    private void Awake()
    {
        area = transform as RectTransform;
        if (handle) startPosition = handle.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData) => UpdateStick(eventData);
    public void OnDrag(PointerEventData eventData) => UpdateStick(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        TouchInput.SetMove(Vector2.zero);
        if (handle) handle.anchoredPosition = startPosition;
    }

    private void UpdateStick(PointerEventData eventData)
    {
        if (!area) return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            return;

        Vector2 halfSize = area.rect.size * 0.5f;
        Vector2 radius = new Vector2(Mathf.Max(1f, halfSize.x), Mathf.Max(1f, halfSize.y));
        Vector2 value = new Vector2(localPoint.x / radius.x, localPoint.y / radius.y);
        TouchInput.SetMove(value);

        if (handle)
        {
            Vector2 limit = area.rect.size * 0.5f * handleRange;
            handle.anchoredPosition = new Vector2(
                Mathf.Clamp(localPoint.x, -limit.x, limit.x),
                Mathf.Clamp(localPoint.y, -limit.y, limit.y));
        }
    }
}