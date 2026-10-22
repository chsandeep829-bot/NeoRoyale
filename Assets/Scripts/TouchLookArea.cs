using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Attach to a transparent UI panel covering the right half of the screen.</summary>
public class TouchLookArea : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Min(0.01f)] public float sensitivity = 0.12f;
    private Vector2 previousPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        previousPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - previousPosition;
        previousPosition = eventData.position;
        TouchInput.AddLookDelta(delta * sensitivity);
    }
}