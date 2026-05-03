using UnityEngine;


///og eNEMY AI SCRIPT
///
/*public class enemy_ai : MonoBehaviour
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
}*/





// WITHOUT ANIMATION 
/*
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("Patrol Settings")]
    public bool isPatrolling = false;
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;

    [Header("Combat")]
    public int damage = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private bool isAttacking = false;

    void Start()
    {
        // Set initial patrol target
        if (pointA != null)
        {
            currentPatrolTarget = pointA;
        }

        // Ensure animator is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Priority: Chase > Attack > Idle/Patrol
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
            // Out of chase range - either patrol or idle
            if (isPatrolling && pointA != null && pointB != null)
            {
                Patrol();
            }
            else
            {
                Idle();
            }
        }
    }

    // -------------------- //
    // IDLE
    // -------------------- //
    void Idle()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;
    }

    // -------------------- //
    // CHASE
    // -------------------- //
    void ChasePlayer()
    {
        MoveTowards(player.position);

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;
    }

    // -------------------- //
    // ATTACK
    // -------------------- //
    void AttackPlayer()
    {
        // Face the player
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        // Stop moving
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }

        // Apply damage with cooldown
        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;

            // Deal damage to player
            actor playerActor = player.GetComponent<actor>();
            if (playerActor != null)
            {
                playerActor.TakeDamage(damage);
                Debug.Log($"Enemy attacked player for {damage} damage!");
            }

            // Reset attack animation after delay
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // -------------------- //
    // PATROL (A ↔ B)
    // -------------------- //
    void Patrol()
    {
        if (currentPatrolTarget == null) return;

        MoveTowards(currentPatrolTarget.position);

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;

        // Check if reached patrol point
        if (Vector3.Distance(transform.position, currentPatrolTarget.position) < 1f)
        {
            // Switch to the other point
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
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

    // -------------------- //
    // DEBUG GIZMOS
    // -------------------- //
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (isPatrolling && pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.position, 0.5f);
        }
    }
}*/




using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    [Header("Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("Patrol Settings")]
    public bool isPatrolling = false;
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;

    [Header("Combat")]
    public int damage = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private bool isAttacking = false;
    private bool isDead = false;
    private bool isHit = false;

    void Start()
    {
        // Set initial patrol target
        if (pointA != null)
        {
            currentPatrolTarget = pointA;
        }

        // Ensure animator is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Initialize health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Don't process any behavior if dead
        if (isDead) return;

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Priority: Chase > Attack > Idle/Patrol
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
            // Out of chase range - either patrol or idle
            if (isPatrolling && pointA != null && pointB != null)
            {
                Patrol();
            }
            else
            {
                Idle();
            }
        }
    }

    // -------------------- //
    // IDLE
    // -------------------- //
    void Idle()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;
    }

    // -------------------- //
    // CHASE
    // -------------------- //
    void ChasePlayer()
    {
        MoveTowards(player.position);

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;
    }

    // -------------------- //
    // ATTACK
    // -------------------- //
    void AttackPlayer()
    {
        // Face the player
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        // Stop moving
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }

        // Apply damage with cooldown
        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;

            // Deal damage to player
            actor playerActor = player.GetComponent<actor>();
            if (playerActor != null)
            {
                playerActor.TakeDamage(damage);
                Debug.Log($"Enemy attacked player for {damage} damage!");
            }

            // Reset attack animation after delay
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // -------------------- //
    // PATROL (A ↔ B)
    // -------------------- //
    void Patrol()
    {
        if (currentPatrolTarget == null) return;

        MoveTowards(currentPatrolTarget.position);

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        isAttacking = false;

        // Check if reached patrol point
        if (Vector3.Distance(transform.position, currentPatrolTarget.position) < 1f)
        {
            // Switch to the other point
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }

    // -------------------- //
    // TAKE DAMAGE (called from sword)
    // -------------------- //
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Enemy Health: {currentHealth}");

        // Play hit animation
        if (animator != null)
        {
            animator.SetTrigger("isHit");
        }

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // -------------------- //
    // DIE
    // -------------------- //
    void Die()
    {
        if (isDead) return;

        isDead = true;

        // Stop moving
        moveSpeed = 0;

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("isDying");
        }

        // Disable collider so player can walk through body
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Destroy the enemy after death animation
        float deathAnimationLength = GetAnimationLength("Dying");
        Destroy(gameObject, deathAnimationLength);
    }

    // Helper to get animation length
    float GetAnimationLength(string animationName)
    {
        if (animator != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Contains(animationName))
                {
                    return clip.length;
                }
            }
        }
        return 2f; // Default fallback
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

    // -------------------- //
    // DEBUG GIZMOS
    // -------------------- //
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (isPatrolling && pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.position, 0.5f);
        }
    }
}