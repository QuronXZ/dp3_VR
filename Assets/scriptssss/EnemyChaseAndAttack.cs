using UnityEngine;

public class EnemyChaseAndAttack : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    // -------------------- //
    // IDLE
    // -------------------- //
    void Idle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    // -------------------- //
    // CHASE
    // -------------------- //
    void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // Rotate smoothly
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        // Move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    // -------------------- //
    // ATTACK
    // -------------------- //
    void AttackPlayer()
    {
        // Stop movement
        animator.SetBool("isWalking", false);

        // Face player
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        animator.SetBool("isAttacking", true);
    }
}