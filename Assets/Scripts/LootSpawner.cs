using UnityEngine;

/// <summary>
/// Spawns configured pickup prefabs around a local test map.
/// Use prefabs with a Collider set to Is Trigger and a Pickup component.
/// </summary>
public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootEntry
    {
        public GameObject prefab;
        [Min(0)] public int count = 3;
    }

    public LootEntry[] loot;
    public Vector3 areaSize = new Vector3(80f, 0f, 80f);
    public Transform[] spawnPoints;
    public bool spawnOnStart = true;
    public bool randomYaw = true;

    private void Start()
    {
        if (spawnOnStart) SpawnLoot();
    }

    public void SpawnLoot()
    {
        if (loot == null || loot.Length == 0) return;

        foreach (LootEntry entry in loot)
        {
            if (entry == null || !entry.prefab || entry.count <= 0) continue;

            for (int i = 0; i < entry.count; i++)
            {
                Vector3 position = GetSpawnPosition();
                Quaternion rotation = randomYaw
                    ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)
                    : Quaternion.identity;
                Instantiate(entry.prefab, position, rotation);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (point) return point.position;
        }

        Vector3 center = transform.position;
        return center + new Vector3(
            Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
            areaSize.y,
            Random.Range(-areaSize.z * 0.5f, areaSize.z * 0.5f));
    }
}