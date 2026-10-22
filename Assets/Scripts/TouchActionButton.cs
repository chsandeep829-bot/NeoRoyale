using UnityEngine;
using UnityEngine.EventSystems;

public class TouchActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ActionType { Fire, Sprint, Jump, Reload }
    public ActionType actionType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (actionType)
        {
            case ActionType.Fire: TouchInput.SetFire(true); break;
            case ActionType.Sprint: TouchInput.SetSprint(true); break;
            case ActionType.Jump: TouchInput.PressJump(); break;
            case ActionType.Reload: TouchInput.PressReload(); break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (actionType == ActionType.Fire) TouchInput.SetFire(false);
        if (actionType == ActionType.Sprint) TouchInput.SetSprint(false);
    }

    private void OnDisable()
    {
        if (actionType == ActionType.Fire) TouchInput.SetFire(false);
        if (actionType == ActionType.Sprint) TouchInput.SetSprint(false);
    }
}