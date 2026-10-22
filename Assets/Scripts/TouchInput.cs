using UnityEngine;

/// <summary>
/// Shared input state written by mobile UI controls and read by gameplay scripts.
/// </summary>
public static class TouchInput
{
    public static Vector2 Move { get; private set; }
    public static Vector2 LookDelta { get; private set; }
    public static bool FireHeld { get; private set; }
    public static bool SprintHeld { get; private set; }

    private static bool jumpPressed;
    private static bool reloadPressed;

    public static void SetMove(Vector2 value) => Move = Vector2.ClampMagnitude(value, 1f);
    public static void AddLookDelta(Vector2 value) => LookDelta += value;
    public static Vector2 ConsumeLookDelta()
    {
        Vector2 value = LookDelta;
        LookDelta = Vector2.zero;
        return value;
    }

    public static void SetFire(bool value) => FireHeld = value;
    public static void SetSprint(bool value) => SprintHeld = value;
    public static void PressJump() => jumpPressed = true;
    public static void PressReload() => reloadPressed = true;

    public static bool ConsumeJump()
    {
        bool value = jumpPressed;
        jumpPressed = false;
        return value;
    }

    public static bool ConsumeReload()
    {
        bool value = reloadPressed;
        reloadPressed = false;
        return value;
    }
}