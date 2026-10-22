using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public float startingRadius = 100f;
    public float finalRadius = 10f;
    [Min(1f)] public float shrinkDuration = 300f;
    [Min(0f)] public float damagePerSecond = 5f;
    public bool startOnPlay = true;

    public float CurrentRadius { get; private set; }
    private float elapsed;
    private bool running;

    private void Start()
    {
        CurrentRadius = Mathf.Max(finalRadius, startingRadius);
        running = startOnPlay;
        UpdateVisualScale();
    }

    private void Update()
    {
        if (!running) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / Mathf.Max(1f, shrinkDuration));
        CurrentRadius = Mathf.Lerp(startingRadius, finalRadius, t);
        UpdateVisualScale();

        Health[] healthObjects = FindObjectsByType<Health>(FindObjectsSortMode.None);
        foreach (Health health in healthObjects)
        {
            if (!health || health.IsDead) continue;
            Vector3 offset = health.transform.position - transform.position;
            offset.y = 0f;

            if (offset.magnitude > CurrentRadius)
                health.TakeDamage(damagePerSecond * Time.deltaTime);
        }

        if (t >= 1f) running = false;
    }

    public void BeginShrinking()
    {
        elapsed = 0f;
        running = true;
    }

    private void UpdateVisualScale()
    {
        // Optional: attach a visible ring/mesh as a child to show the boundary.
        // Keep this manager object's scale unchanged so its position remains the zone center.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float radius = Application.isPlaying ? CurrentRadius : startingRadius;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}