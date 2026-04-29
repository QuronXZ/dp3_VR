using UnityEngine;

public class enemy_ai : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Ranges")]
    public float sightRange = 10f;
    public float stopDistance = 2f;

    [Header("Patrol A-B")]
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;

    void Start()
    {
        currentTarget = pointA;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < sightRange)
        {
            ChasePlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    // -------------------- //
    // PATROL (A ↔ B)
    // -------------------- //
    void Patrol()
    {
        MoveTowards(currentTarget.position);

        if (Vector3.Distance(transform.position, currentTarget.position) < 3f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    // -------------------- //
    // CHASE
    // -------------------- //
    void ChasePlayer(float distance)
    {
        if (distance > stopDistance)
        {
            MoveTowards(player.position);
        }
        else
        {
            Patrol();
        }
    }

    // -------------------- //
    // CORE MOVEMENT
    // -------------------- //
    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}