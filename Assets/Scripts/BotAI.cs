using UnityEngine;

[RequireComponent(typeof(Health))]
public class BotAI : MonoBehaviour
{
    public Transform target;
    [Min(0f)] public float moveSpeed = 2.8f;
    [Min(1f)] public float detectionRange = 25f;
    [Min(1f)] public float attackRange = 12f;
    [Min(0.1f)] public float attackInterval = 1.4f;
    [Min(0f)] public float attackDamage = 8f;
    public Transform eyePoint;

    private Health ownHealth;
    private float nextAttackTime;

    private void Awake()
    {
        ownHealth = GetComponent<Health>();
    }

    private void Start()
    {
        FindPlayerIfNeeded();
    }

    private void Update()
    {
        if (ownHealth.IsDead) return;
        if (!target) FindPlayerIfNeeded();
        if (!target) return;

        Health targetHealth = target.GetComponentInParent<Health>();
        if (!targetHealth || targetHealth.IsDead) return;

        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
        if (distance > detectionRange) return;

        Vector3 flatDirection = new Vector3(toTarget.x, 0f, toTarget.z).normalized;
        if (flatDirection.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatDirection);

        if (distance > attackRange)
        {
            transform.position += flatDirection * moveSpeed * Time.deltaTime;
        }
        else if (Time.time >= nextAttackTime && HasLineOfSight())
        {
            nextAttackTime = Time.time + attackInterval;
            targetHealth.TakeDamage(attackDamage);
        }
    }

    private bool HasLineOfSight()
    {
        Vector3 origin = eyePoint ? eyePoint.position : transform.position + Vector3.up;
        Vector3 destination = target.position + Vector3.up;
        Vector3 direction = destination - origin;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit,
            direction.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            return hit.transform == target || hit.transform.IsChildOf(target);
        }

        return true;
    }

    private void FindPlayerIfNeeded()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) target = player.transform;
    }
}