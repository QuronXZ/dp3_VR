using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Section Settings")]
    public List<GameObject> enemiesInSection; // Drag enemy GameObjects here
    public GameObject gate; // The gate to open when all enemies are dead

    [Header("Gate Settings")]
    public bool startGateLocked = true;
    public string unlockMessage = "Gate Unlocked!";

    private int enemiesRemaining;
    private bool gateUnlocked = false;

    void Start()
    {
        // Count initial enemies
        enemiesRemaining = enemiesInSection.Count;

        // Close/lock gate at start
        if (startGateLocked && gate != null)
        {
            LockGate();
        }

        Debug.Log($"Section started with {enemiesRemaining} enemies remaining");
    }

    public void EnemyDied(GameObject enemy)
    {
        // Check if this enemy is in our list
        if (enemiesInSection.Contains(enemy))
        {
            enemiesRemaining--;
            Debug.Log($"Enemy defeated! {enemiesRemaining} enemies remaining");

            // Check if all enemies are dead
            if (enemiesRemaining <= 0)
            {
                AllEnemiesDefeated();
            }
        }
    }

    void AllEnemiesDefeated()
    {
        Debug.Log("All enemies defeated! Opening gate...");
        OpenGate();
    }

    void OpenGate()
    {
        if (gate != null && !gateUnlocked)
        {
            gateUnlocked = true;

            // Disable gate collider so player can pass
            Collider gateCollider = gate.GetComponent<Collider>();
            MeshRenderer meshr = gate.GetComponent<MeshRenderer>();
            if (gateCollider != null)
            {
                meshr.enabled = false;
                gateCollider.enabled = false;
                
            }

            // Optional: Play open animation
            Animator gateAnimator = gate.GetComponent<Animator>();
            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("Open");
            }

            Debug.Log(unlockMessage);
        }
    }

    void LockGate()
    {
        if (gate != null)
        {
            Collider gateCollider = gate.GetComponent<Collider>();
            if (gateCollider != null)
            {
                gateCollider.enabled = true;
            }

            Animator gateAnimator = gate.GetComponent<Animator>();
            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("Close");
            }
        }
    }
}