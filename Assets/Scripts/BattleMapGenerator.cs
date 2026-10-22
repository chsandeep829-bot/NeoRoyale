using UnityEngine;

/// <summary>
/// Builds a simple placeholder battle arena from Unity primitives at runtime.
/// Replace these blocks with authored art and optimized meshes for a real release.
/// </summary>
public class BattleMapGenerator : MonoBehaviour
{
    [Header("Arena")]
    [Min(40f)] public float mapSize = 180f;
    [Min(1f)] public float groundThickness = 1f;
    public bool generateOnStart = true;
    public bool createBoundaryWalls = true;

    [Header("Layout")]
    [Min(0)] public int buildingCount = 8;
    [Min(0)] public int coverCount = 24;
    [Min(8f)] public float minimumFeatureSpacing = 10f;
    [Min(0f)] public float edgeMargin = 12f;
    [Min(1f)] public float buildingMinHeight = 4f;
    [Min(1f)] public float buildingMaxHeight = 9f;
    [Min(1f)] public float coverMinHeight = 1f;
    [Min(1f)] public float coverMaxHeight = 2.2f;
    public Material groundMaterial;
    public Material buildingMaterial;
    public Material coverMaterial;
    public Material boundaryMaterial;

    private void Start()
    {
        if (generateOnStart) Generate();
    }

    [ContextMenu("Generate Battle Map")]
    public void Generate()
    {
        ClearGenerated();
        CreateBlock("Arena Ground", new Vector3(0f, -groundThickness * 0.5f, 0f),
            new Vector3(mapSize, groundThickness, mapSize), groundMaterial, true);

        if (createBoundaryWalls) CreateBoundaries();

        Vector3[] reserved = new Vector3[buildingCount + coverCount];
        int reservedCount = 0;

        for (int i = 0; i < buildingCount; i++)
        {
            Vector3 point = FindPosition(reserved, reservedCount);
            reserved[reservedCount++] = point;
            float width = Random.Range(8f, 15f);
            float depth = Random.Range(8f, 15f);
            float height = Random.Range(buildingMinHeight, Mathf.Max(buildingMinHeight, buildingMaxHeight));
            CreateBlock("Building Placeholder " + (i + 1),
                new Vector3(point.x, height * 0.5f, point.z),
                new Vector3(width, height, depth), buildingMaterial, true);
        }

        for (int i = 0; i < coverCount; i++)
        {
            Vector3 point = FindPosition(reserved, reservedCount);
            reserved[reservedCount++] = point;
            float width = Random.Range(2f, 5f);
            float depth = Random.Range(1f, 3f);
            float height = Random.Range(coverMinHeight, Mathf.Max(coverMinHeight, coverMaxHeight));
            if (Random.value > 0.5f)
            {
                float swap = width;
                width = depth;
                depth = swap;
            }

            CreateBlock("Cover Placeholder " + (i + 1),
                new Vector3(point.x, height * 0.5f, point.z),
                new Vector3(width, height, depth), coverMaterial, true);
        }
    }

    private Vector3 FindPosition(Vector3[] reserved, int count)
    {
        float half = mapSize * 0.5f - edgeMargin;
        for (int attempt = 0; attempt < 40; attempt++)
        {
            Vector3 candidate = new Vector3(Random.Range(-half, half), 0f, Random.Range(-half, half));
            bool clear = true;
            for (int i = 0; i < count; i++)
            {
                if (Vector3.Distance(candidate, reserved[i]) < minimumFeatureSpacing)
                {
                    clear = false;
                    break;
                }
            }

            if (clear) return candidate;
        }

        return new Vector3(Random.Range(-half, half), 0f, Random.Range(-half, half));
    }

    private void CreateBoundaries()
    {
        float half = mapSize * 0.5f;
        float wallHeight = 5f;
        float wallThickness = 2f;

        CreateBlock("Boundary North", new Vector3(0f, wallHeight * 0.5f, half),
            new Vector3(mapSize, wallHeight, wallThickness), boundaryMaterial, true);
        CreateBlock("Boundary South", new Vector3(0f, wallHeight * 0.5f, -half),
            new Vector3(mapSize, wallHeight, wallThickness), boundaryMaterial, true);
        CreateBlock("Boundary East", new Vector3(half, wallHeight * 0.5f, 0f),
            new Vector3(wallThickness, wallHeight, mapSize), boundaryMaterial, true);
        CreateBlock("Boundary West", new Vector3(-half, wallHeight * 0.5f, 0f),
            new Vector3(wallThickness, wallHeight, mapSize), boundaryMaterial, true);
    }

    private void CreateBlock(string objectName, Vector3 position, Vector3 scale, Material material, bool addCollider)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = objectName;
        block.transform.SetParent(transform, false);
        block.transform.position = transform.position + position;
        block.transform.localScale = scale;

        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer && material) renderer.sharedMaterial = material;

        if (!addCollider)
        {
            Collider collider = block.GetComponent<Collider>();
            if (collider) Destroy(collider);
        }
    }

    private void ClearGenerated()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(child);
            else DestroyImmediate(child);
        }
    }
}