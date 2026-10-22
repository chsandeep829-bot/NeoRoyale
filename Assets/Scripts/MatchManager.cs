using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Local prototype match loop: spawn bots, track player survival, and show a result screen.
/// This is not a network-authoritative multiplayer manager.
/// </summary>
public class MatchManager : MonoBehaviour
{
    [Header("Match Setup")]
    public Transform player;
    public GameObject botPrefab;
    [Min(0)] public int botsToSpawn = 8;
    [Min(0f)] public float spawnRadius = 45f;
    public Transform[] spawnPoints;
    public bool spawnBotsOnStart = true;
    public MatchTimer matchTimer;

    [Header("Result Screen")]
    public bool pauseOnMatchEnd = true;
    public string winMessage = "VICTORY!";
    public string loseMessage = "ELIMINATED";

    private Health playerHealth;
    private bool matchEnded;
    private bool playerWon;
    private int spawnedBots;

    public bool MatchEnded => matchEnded;
    public int SpawnedBots => spawnedBots;

    private void Start()
    {
        ResolvePlayer();
        if (!matchTimer) matchTimer = FindFirstObjectByType<MatchTimer>();
        if (player && spawnBotsOnStart)
            SpawnBots();
    }

    private void Update()
    {
        if (matchEnded) return;

        ResolvePlayer();
        if (!player)
        {
            EndMatch(false);
            return;
        }

        if (!playerHealth)
            playerHealth = player.GetComponentInParent<Health>();

        if (playerHealth && playerHealth.IsDead)
        {
            EndMatch(false);
            return;
        }

        // If the configured countdown expires while the player is alive, finish the match.
        if (!matchTimer) matchTimer = FindFirstObjectByType<MatchTimer>();
        if (matchTimer && matchTimer.countDown && matchTimer.RemainingSeconds <= 0f)
        {
            EndMatch(true);
            return;
        }

        BotAI[] bots = FindObjectsByType<BotAI>(FindObjectsSortMode.None);
        bool anyLivingBot = false;
        foreach (BotAI bot in bots)
        {
            if (bot && bot.enabled)
            {
                Health health = bot.GetComponent<Health>();
                if (!health || !health.IsDead)
                {
                    anyLivingBot = true;
                    break;
                }
            }
        }

        if (spawnedBots > 0 && !anyLivingBot)
            EndMatch(true);
    }

    public void SpawnBots()
    {
        if (!botPrefab)
        {
            Debug.LogWarning("MatchManager: assign a Bot Prefab before spawning bots.", this);
            return;
        }

        ResolvePlayer();
        for (int i = 0; i < botsToSpawn; i++)
        {
            Vector3 position = GetSpawnPosition(i);
            GameObject instance = Instantiate(botPrefab, position, Quaternion.identity);
            BotAI bot = instance.GetComponent<BotAI>();
            if (bot && player)
                bot.target = player;
            spawnedBots++;
        }
    }

    private Vector3 GetSpawnPosition(int index)
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform point = spawnPoints[index % spawnPoints.Length];
            if (point) return point.position;
        }

        Vector3 center = player ? player.position : transform.position;
        Vector2 offset = Random.insideUnitCircle;
        if (offset.sqrMagnitude < 0.01f) offset = Vector2.right;
        offset = offset.normalized * Random.Range(spawnRadius * 0.55f, spawnRadius);
        return center + new Vector3(offset.x, 0f, offset.y);
    }

    private void ResolvePlayer()
    {
        if (!player)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found) player = found.transform;
        }

        if (player && !playerHealth)
            playerHealth = player.GetComponentInParent<Health>();
    }

    private void EndMatch(bool won)
    {
        if (matchEnded) return;
        matchEnded = true;
        playerWon = won;
        if (pauseOnMatchEnd) Time.timeScale = 0f;
    }

    private void OnGUI()
    {
        if (!matchEnded) return;

        float panelWidth = Mathf.Min(420f, Screen.width - 32f);
        float panelHeight = 190f;
        Rect panel = new Rect((Screen.width - panelWidth) * 0.5f,
            (Screen.height - panelHeight) * 0.5f, panelWidth, panelHeight);

        GUI.Box(panel, "");
        GUIStyle title = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Clamp(Screen.width / 28, 22, 40),
            fontStyle = FontStyle.Bold
        };
        GUI.Label(new Rect(panel.x + 12f, panel.y + 20f, panel.width - 24f, 65f),
            playerWon ? winMessage : loseMessage, title);

        if (GUI.Button(new Rect(panel.x + 30f, panel.y + 105f, panel.width - 60f, 34f), "PLAY AGAIN"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (GUI.Button(new Rect(panel.x + 30f, panel.y + 145f, panel.width - 60f, 28f), "CONTINUE TESTING"))
        {
            Time.timeScale = 1f;
            matchEnded = false;
        }
    }

    private void OnDisable()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }
}