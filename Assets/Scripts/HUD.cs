using UnityEngine;

public class HUD : MonoBehaviour
{
    public Health playerHealth;
    public WeaponController weapon;
    public SafeZone safeZone;

    private GUIStyle labelStyle;
    private GUIStyle crosshairStyle;

    private void Awake()
    {
        if (!playerHealth) playerHealth = GetComponent<Health>();
        if (!weapon) weapon = GetComponent<WeaponController>();
        if (!safeZone) safeZone = FindFirstObjectByType<SafeZone>();
    }

    private void OnGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Max(16, Screen.height / 40),
                normal = { textColor = Color.white }
            };
            crosshairStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }

        float y = 16f;
        if (playerHealth)
        {
            GUI.Label(new Rect(16, y, 300, 32),
                $"HEALTH: {Mathf.CeilToInt(playerHealth.CurrentHealth)}", labelStyle);
            y += 34f;

            if (playerHealth.IsDead)
                GUI.Label(new Rect(16, y, 300, 40), "ELIMINATED", labelStyle);
        }

        if (weapon)
        {
            string reloadText = weapon.IsReloading ? "  RELOADING..." : "";
            GUI.Label(new Rect(16, y, 400, 32),
                $"AMMO: {weapon.AmmoInMagazine} / {weapon.reserveAmmo}{reloadText}", labelStyle);
        }

        if (safeZone)
        {
            GUI.Label(new Rect(16, Screen.height - 42, 420, 32),
                $"SAFE ZONE RADIUS: {Mathf.RoundToInt(safeZone.CurrentRadius)}", labelStyle);
        }

        GUI.Label(new Rect(Screen.width / 2f - 20f, Screen.height / 2f - 20f, 40, 40),
            "+", crosshairStyle);
    }
}