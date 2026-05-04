using System.Collections.Generic;
using UnityEngine;

public class GameManager_lvl2 : MonoBehaviour

{
    [Header("Section Settings")]
    public List<GameObject> enemiesInSection; // Drag enemy GameObjects here
    public GameObject gate; // The gate to open when all enemies are dead

    [Header("Card Settings")]
    public CardParentManager cardParentManager;

    //public CardTrigger[] cardTriggers; 

    [Header("Gate Settings")]
    public bool startGateLocked = true;
    public string unlockMessage = "Gate Unlocked!";


    [Header("Gate Animation")]
    public float openHeight = 0.5f; // How high the gate moves up
    public float openSpeed = 2f; // Speed of the lerp movement

    //[Header("Gate Sounds")]
    //public AudioSource gateAudioSource; // First audio source for movement sound
    //public AudioClip gate1, gate2;
    private Vector3 gateClosedPosition;

    private int enemiesRemaining;
    private bool gateUnlocked = false;
    private bool isGateOpening = false;
    private Dictionary<int, bool> cardClosedStatus = new Dictionary<int, bool>();

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


    void Update()
    {
        // Handle gate opening animation in Update
        if (isGateOpening && gate != null && !gateUnlocked)
        {
            Vector3 targetPosition = gateClosedPosition + new Vector3(0, openHeight, 0);
            gate.transform.position = Vector3.Lerp(gate.transform.position, targetPosition, Time.deltaTime * openSpeed);

            // Check if gate reached target position
            if (Vector3.Distance(gate.transform.position, targetPosition) < 0.01f)
            {
                isGateOpening = false;
                gateUnlocked = true;
                Debug.Log("Gate fully opened!");
            }
        }
    }


    // Call this from Trigger Zone script
    public void ShowCardFromTrigger(int cardIndex)
    {
        if (cardParentManager != null)
        {
            Debug.Log($"GameManager: Showing card {cardIndex} from trigger");
            cardParentManager.ForceShowCard(cardIndex);
        }
        else
        {
            Debug.LogError("CardParentManager not assigned in GameManager!");
        }
    }

    // Call this when card is closed (from CardParentManager)
    public void OnCardClosed(int cardIndex)
    {
        cardClosedStatus[cardIndex] = true;
        Debug.Log($"Card {cardIndex} closed");
    }

    // Check if card is closed
    public bool IsCardClosed(int cardIndex)
    {
        if (cardClosedStatus.ContainsKey(cardIndex))
            return cardClosedStatus[cardIndex];
        return false;
    }

    public void EnemyDied(GameObject enemy)
    {
        // Check if this enemy is in our list
        if (enemiesInSection.Contains(enemy))
        {
            enemiesRemaining--;
            if (enemiesRemaining == 3)
            {
                Debug.Log($"Crate broken! Showing card ");
                cardParentManager.ForceShowCard(4);
            }
            Debug.Log($"Enemy defeated! {enemiesRemaining} enemies remaining");

            // Check if all enemies are dead
            if (enemiesRemaining <= 0)
            {
                AllEnemiesDefeated();
            }
        }
    }



    public void AllEnemiesDefeated()
    {
        Debug.Log("All enemies defeated! Opening gate...");
        OpenGate();
    }
    /*
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

                    Vector3 targetPosition = gateClosedPosition + new Vector3(0, openHeight, 0);
                    gate.transform.position = Vector3.Lerp(gate.transform.position, targetPosition, Time.deltaTime * openSpeed);
                    //meshr.enabled = false;
                    gateCollider.enabled = false;
                    Audio_manager.Instance.PlaySFX(0);
                    Audio_manager.Instance.PlaySFX(1);


                }

                // Optional: Play open animation
                Animator gateAnimator = gate.GetComponent<Animator>();
                if (gateAnimator != null)
                {
                    gateAnimator.SetTrigger("Open");
                }

                Debug.Log(unlockMessage);
            }
        }*/
    void OpenGate()
    {
        if (gate != null && !gateUnlocked && !isGateOpening)
        {
            // Disable gate collider so player can pass
            Collider gateCollider = gate.GetComponent<Collider>();
            if (gateCollider != null)
            {
                gateCollider.enabled = false;
            }

            // Play sounds
            Audio_manager.Instance.PlaySFX(0);
            Audio_manager.Instance.PlaySFX(1);

            // Start gate animation
            isGateOpening = true;

            // Optional: Play open animation
            Animator gateAnimator = gate.GetComponent<Animator>();
            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("Open");
            }

            cardParentManager.ForceShowCard(9);

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