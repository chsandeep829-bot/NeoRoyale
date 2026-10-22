using UnityEngine;

/// <summary>
/// Optional compact match-status overlay. Add to a scene object and assign references,
/// or let it locate common prototype components automatically.
/// </summary>
public class MatchStatusHUD : MonoBehaviour
{
    public MatchTimer matchTimer;
    public MatchManager matchManager;
    public Health playerHealth;
    public WeaponController weapon;
    public bool showAliveBots = true;

    private GUIStyle style;
    private GUIStyle titleStyle;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (!matchTimer) matchTimer = FindFirstObjectByType<MatchTimer>();
        if (!matchManager) matchManager = FindFirstObjectByType<MatchManager>();
        if (!playerHealth)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player) playerHealth = player.GetComponentInParent<Health>();
        }
        if (!weapon && playerHealth)
            weapon = playerHealth.GetComponent<WeaponController>();
    }

    private void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Clamp(Screen.height / 48, 14, 24),
                normal = { textColor = Color.white }
            };
            titleStyle = new GUIStyle(style) { fontStyle = FontStyle.Bold };
        }

        ResolveReferences();
        float x = Screen.width - 220f;
        float y = 16f;
        GUI.Label(new Rect(x, y, 200f, 30f), "NEOROYALE", titleStyle);
        y += 30f;

        if (matchTimer)
        {
            GUI.Label(new Rect(x, y, 200f, 28f), "TIME  " + matchTimer.GetFormattedTime(), style);
            y += 28f;
        }

        if (playerHealth)
        {
            GUI.Label(new Rect(x, y, 200f, 28f),
                "HP  " + Mathf.CeilToInt(playerHealth.CurrentHealth), style);
            y += 28f;
        }

        if (weapon)
        {
            GUI.Label(new Rect(x, y, 200f, 28f),
                "AMMO  " + weapon.AmmoInMagazine + " / " + weapon.reserveAmmo, style);
            y += 28f;
        }

        if (showAliveBots)
        {
            int alive = 0;
            BotAI[] bots = FindObjectsByType<BotAI>(FindObjectsSortMode.None);
            foreach (BotAI bot in bots)
            {
                if (!bot) continue;
                Health health = bot.GetComponent<Health>();
                if (!health || !health.IsDead) alive++;
            }

            GUI.Label(new Rect(x, y, 200f, 28f), "BOTS  " + alive, style);
        }
    }
}
