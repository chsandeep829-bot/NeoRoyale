using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class BotAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    [Min(1f)] public float detectionRange = 35f;
    [Min(1f)] public float attackRange = 12f;
    [Min(0.1f)] public float attackInterval = 1.4f;
    [Min(0f)] public float attackDamage = 8f;
    public Transform eyePoint;

    [Header("Movement")]
    [Min(0f)] public float moveSpeed = 3.2f;
    public bool useNavMeshWhenAvailable = true;
    [Min(0.1f)] public float destinationRefreshInterval = 0.25f;
    [Min(0f)] public float stoppingDistance = 8f;

    private Health ownHealth;
    private NavMeshAgent agent;
    private float nextAttackTime;
    private float nextDestinationTime;

    private void Awake()
    {
        ownHealth = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
        }
    }

    private void Start()
    {
        FindPlayerIfNeeded();
    }

    private void Update()
    {
        if (!ownHealth || ownHealth.IsDead) return;
        if (!target) FindPlayerIfNeeded();
        if (!target) { StopMovement(); return; }

        Health targetHealth = target.GetComponentInParent<Health>();
        if (!targetHealth || targetHealth.IsDead) { StopMovement(); return; }

        Vector3 toTarget = target.position - transform.position;
        Vector3 flatDirection = new Vector3(toTarget.x, 0f, toTarget.z);
        float distance = flatDirection.magnitude;

        if (distance > detectionRange)
        {
            StopMovement();
            return;
        }

        if (flatDirection.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(flatDirection.normalized), Time.deltaTime * 8f);

        if (distance > attackRange)
        {
            MoveTowardTarget();
        }
        else
        {
            StopMovement();
            if (Time.time >= nextAttackTime && HasLineOfSight())
            {
                nextAttackTime = Time.time + attackInterval;
                targetHealth.TakeDamage(attackDamage);
            }
        }
    }

    private void MoveTowardTarget()
    {
        if (useNavMeshWhenAvailable && agent && agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = Mathf.Max(0.1f, stoppingDistance);
            if (Time.time >= nextDestinationTime)
            {
                agent.SetDestination(target.position);
                nextDestinationTime = Time.time + destinationRefreshInterval;
            }
            return;
        }

        // Simple fallback for scenes that do not have a baked NavMesh.
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void StopMovement()
    {
        if (agent && agent.enabled && agent.isOnNavMesh)
            agent.ResetPath();
    }

    private bool HasLineOfSight()
    {
        Vector3 origin = eyePoint ? eyePoint.position : transform.position + Vector3.up;
        Vector3 destination = target.position + Vector3.up;
        Vector3 direction = destination - origin;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit,
            direction.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            Transform hitTransform = hit.transform;
            return hitTransform == target || hitTransform.IsChildOf(target) ||
                   target.IsChildOf(hitTransform);
        }

        return true;
    }

    private void FindPlayerIfNeeded()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) target = player.transform;
    }
}