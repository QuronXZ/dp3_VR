using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
/*public class actor : MonoBehaviour


{
    int currentHealth;
    public int maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: "+ currentHealth);
        if (currentHealth <= 0)
        { Death(); }
    }

    void Death()
    {
        // Death function
        // TEMPORARY: Destroy Object
        Destroy(gameObject);
    }
}
*/

/*
public class actor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public GameManager manager;

    [Header("Slicing Settings")]
    public bool enableSlicingOnLastHit = true;
    public string normalTag = "hittable";
    public string sliceableTag = "Sliceable";

    // Reference to sliceable component (will be added when needed)
    private Sliceable sliceableComponent;
    private bool isSlicingEnabled = false;

    void Awake()
    {
        currentHealth = maxHealth;

        // Start with hittable tag
        gameObject.tag = normalTag;

        // Check if Sliceable component exists, if not we'll add it later
        sliceableComponent = GetComponent<Sliceable>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);

        // Check if this is the last hit (health will become 0 or less)
        if (enableSlicingOnLastHit && currentHealth <= 1 && !isSlicingEnabled)
        {
            EnableSlicingForFinalHit();
        }


        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void EnableSlicingForFinalHit()
    {
        Debug.Log("Final hit! Enemy will be sliced!");

        // Change tag to sliceable
        gameObject.tag = sliceableTag;
        //isSlicingEnabled = true;

        // Add Sliceable component if not present
        if (sliceableComponent == null)
        {
            sliceableComponent = gameObject.AddComponent<Sliceable>();

            // Configure sliceable settings
            sliceableComponent.IsSolid = true;
            sliceableComponent.UseGravity = true;
            sliceableComponent.ReverseWireTriangles = false;
            sliceableComponent.ShareVertices = false;
            sliceableComponent.SmoothVertices = true;
        }

        // Ensure the object has necessary components for slicing
        //EnsureSlicingComponents();

        // Optional: Visual feedback that enemy is now sliceable
        //StartCoroutine(FlashRedBeforeSlice());
    }

    void EnsureSlicingComponents()
    {
        // Add MeshFilter if missing (for slicing)
        if (GetComponent<MeshFilter>() == null)
        {
            // For skinned mesh characters, this is trickier
            SkinnedMeshRenderer skinnedMesh = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMesh != null)
            {
                // Bake skinned mesh to static mesh for slicing
                Mesh bakedMesh = new Mesh();
                skinnedMesh.BakeMesh(bakedMesh);

                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.mesh = bakedMesh;

                // Disable skinned mesh renderer, enable static mesh renderer
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.materials = skinnedMesh.materials;
                skinnedMesh.enabled = false;
            }
        }

        // Ensure collider exists and is not a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        // Ensure rigidbody exists
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    IEnumerator FlashRedBeforeSlice()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            renderer.material.color = originalColor;
        }
    }

    void Death()
    {
        // Only destroy if not already being sliced
        if (!isSlicingEnabled)
        {
            manager.EnemyDied(gameObject);
            Destroy(gameObject);
        }
        else
        {
            // Let the slicing system handle destruction
            Debug.Log("Enemy will be sliced instead of destroyed");
        }
    }
}*/






[RequireComponent(typeof(Collider))]
public class actor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public GameManager manager;

    [Header("Slicing Settings")]
    public bool enableSlicingOnLastHit = true;
    public string normalTag = "hittable";
    public string sliceableTag = "Sliceable";

    [Header("Gate Unlock Settings")]
    public bool requiresMultipleHitsForGate = false;
    public int hitsRequiredForGate = 15; // Number of hits needed before gate can unlock
    private int currentHitCount = 0;
    private bool hasNotifiedGateReady = false;

    // Reference to sliceable component (will be added when needed)
    private Sliceable sliceableComponent;
    private bool isSlicingEnabled = false;

    void Awake()
    {
        currentHealth = maxHealth;

        // Start with hittable tag
        gameObject.tag = normalTag;

        // Check if Sliceable component exists, if not we'll add it later
        sliceableComponent = GetComponent<Sliceable>();

        // Auto-detect if this enemy requires multiple hits for gate
        if (maxHealth >= 100)
        {
            requiresMultipleHitsForGate = true;
            Debug.Log($"Enemy has {maxHealth} health. Will notify gate after {hitsRequiredForGate} hits.");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);

        // Track hits for gate unlocking (only if not already notified)
        if (requiresMultipleHitsForGate && !hasNotifiedGateReady)
        {
            currentHitCount++;
            Debug.Log($"Hit count: {currentHitCount}/{hitsRequiredForGate}");

            // Check if enough hits landed to unlock gate
            if (currentHitCount >= hitsRequiredForGate && !hasNotifiedGateReady)
            {
                NotifyGateReady();
            }
        }

        // Check if this is the last hit (health will become 0 or less)
        if (enableSlicingOnLastHit && currentHealth <= 1 && !isSlicingEnabled)
        {
            EnableSlicingForFinalHit();
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void NotifyGateReady()
    {
        hasNotifiedGateReady = true;

        if (manager != null)
        {
            manager.EnemyDied(gameObject);
            Debug.Log("Gate unlock condition met! Notified GameManager.");
        }
        else
        {
            Debug.LogWarning("GameManager reference missing! Cannot notify gate.");
        }
    }

    void EnableSlicingForFinalHit()
    {
        Debug.Log("Final hit! Enemy will be sliced!");
        isSlicingEnabled = true;

        // Change tag to sliceable
        gameObject.tag = sliceableTag;

        // Add Sliceable component if not present
        if (sliceableComponent == null)
        {
            sliceableComponent = gameObject.AddComponent<Sliceable>();

            // Configure sliceable settings
            sliceableComponent.IsSolid = true;
            sliceableComponent.UseGravity = true;
            sliceableComponent.ReverseWireTriangles = false;
            sliceableComponent.ShareVertices = false;
            sliceableComponent.SmoothVertices = true;
        }
    }

    void Death()
    {
        // Only destroy if not already being sliced
        if (!isSlicingEnabled)
        {
            // Only notify GameManager if we haven't already (for multi-hit enemies)
            if (!hasNotifiedGateReady)
            {
                if (manager != null)
                {
                    manager.EnemyDied(gameObject);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            // Let the slicing system handle destruction
            Debug.Log("Enemy will be sliced instead of destroyed");
        }
    }
}