using UnityEngine;

/// <summary>
/// Optional quick-start scene builder for testing the prototype without manually
/// placing every core object. Add this component to an empty scene object.
/// The generated world is a graybox, not production-ready art.
/// </summary>
[DefaultExecutionOrder(-1000)]
public class PrototypeSceneBootstrap : MonoBehaviour
{
    [Header("Quick Start")]
    [Min(0)] public int botCount = 8;
    [Min(20f)] public float arenaSize = 180f;
    [Min(5f)] public float botSpawnRadius = 55f;
    public bool createOnAwake = true;

    private void Awake()
    {
        if (createOnAwake)
            BuildPrototype();
    }

    [ContextMenu("Build Prototype Scene")]
    public void BuildPrototype()
    {
        // Avoid creating a second set if this method is invoked more than once.
        if (GameObject.Find("NEOROYALE_RuntimePlayer"))
        {
            Debug.LogWarning("NEOROYALE prototype objects already exist in this scene.", this);
            return;
        }

        CreateCamera();

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "NEOROYALE_Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(arenaSize / 10f, 1f, arenaSize / 10f);

        GameObject mapObject = new GameObject("NEOROYALE_BattleMap");
        BattleMapGenerator map = mapObject.AddComponent<BattleMapGenerator>();
        map.mapSize = arenaSize;
        map.buildingCount = 10;
        map.coverCount = 28;
        map.generateOnStart = true;

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "NEOROYALE_RuntimePlayer";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 1.1f, 0f);
        Collider oldCollider = player.GetComponent<Collider>();
        if (oldCollider) Destroy(oldCollider);
        CharacterController characterController = player.AddComponent<CharacterController>();
        characterController.height = 2f;
        characterController.radius = 0.38f;
        characterController.center = new Vector3(0f, 0f, 0f);
        player.AddComponent<Health>();
        player.AddComponent<PlayerController>();
        player.AddComponent<WeaponController>();
        player.AddComponent<HUD>();

        GameObject systems = new GameObject("NEOROYALE_MatchSystems");
        MatchTimer timer = systems.AddComponent<MatchTimer>();
        timer.matchDuration = 600f;
        timer.countDown = true;

        SafeZone safeZone = systems.AddComponent<SafeZone>();
        safeZone.startingRadius = arenaSize * 0.48f;
        safeZone.finalRadius = 8f;
        safeZone.shrinkDuration = 300f;
        safeZone.damagePerSecond = 5f;

        MatchManager manager = systems.AddComponent<MatchManager>();
        manager.player = player.transform;
        manager.matchTimer = timer;
        manager.botsToSpawn = 0;
        manager.spawnBotsOnStart = false;
        manager.pauseOnMatchEnd = true;

        MatchStatusHUD status = systems.AddComponent<MatchStatusHUD>();
        status.matchTimer = timer;
        status.matchManager = manager;
        status.playerHealth = player.GetComponent<Health>();
        status.weapon = player.GetComponent<WeaponController>();

        for (int i = 0; i < botCount; i++)
        {
            float angle = (Mathf.PI * 2f * i) / Mathf.Max(1, botCount);
            float radius = Random.Range(botSpawnRadius * 0.65f, botSpawnRadius);
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 1f, Mathf.Sin(angle) * radius);

            GameObject bot = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            bot.name = "NEOROYALE_Bot_" + (i + 1);
            bot.transform.position = position;
            bot.AddComponent<Health>();
            BotAI ai = bot.AddComponent<BotAI>();
            ai.target = player.transform;
            ai.detectionRange = 60f;
            ai.attackRange = 10f;
            ai.moveSpeed = 2.8f;
            ai.attackDamage = 7f;
            manager.RegisterSpawnedBot(bot);
        }

        Debug.Log("NEOROYALE graybox scene created. Desktop controls: WASD, mouse, left-click to fire, R to reload, Shift to sprint, Space to jump.", this);
    }

    private static void CreateCamera()
    {
        Camera existingCamera = Camera.main;
        if (existingCamera) return;

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        cameraObject.transform.position = new Vector3(0f, 3f, -6f);
        cameraObject.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
    }
}
