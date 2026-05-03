using UnityEngine;


public class SwordDirectionSlice : MonoBehaviour
{
/*    [Header("Sword Settings")]
    public float minSwingSpeed = 1.5f;
    public float sliceCooldown = 0.2f;

    [Header("Blade References")]
    public Transform bladeTip;
    public Transform bladeBase;

    // Tracking variables
    private Vector3 lastTipPosition;
    private Vector3 lastBasePosition;
    private float currentSwingSpeed;
    private float lastSliceTime;

    // Store slice data
    private Vector3 enterTipPosition;
    private Vector3 enterBasePosition;
    private GameObject currentTarget;
    private bool isInTarget = false;

    void Start()
    {
        if (bladeTip == null || bladeBase == null)
        {
            Debug.LogError("Assign blade tip and base transforms on your sword!");
        }

        lastTipPosition = bladeTip.position;
        lastBasePosition = bladeBase.position;
    }

    void Update()
    {
        // Calculate swing speed
        float tipSpeed = (bladeTip.position - lastTipPosition).magnitude / Time.deltaTime;
        float baseSpeed = (bladeBase.position - lastBasePosition).magnitude / Time.deltaTime;
        currentSwingSpeed = (tipSpeed + baseSpeed) / 2f;

        // Update last positions
        lastTipPosition = bladeTip.position;
        lastBasePosition = bladeBase.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only slice objects with "Sliceable" tag
        if (!other.CompareTag("Sliceable")) return;

        // Store entry data
        enterTipPosition = bladeTip.position;
        enterBasePosition = bladeBase.position;
        currentTarget = other.gameObject;
        isInTarget = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Only process if this is the target we entered
        if (!isInTarget || other.gameObject != currentTarget) return;
        if (!other.CompareTag("Sliceable")) return;

        // Check if swing is fast enough
        if (currentSwingSpeed < minSwingSpeed)
        {
            Debug.Log("Swing too slow to slice!");
            isInTarget = false;
            return;
        }

        // Check cooldown
        if (Time.time - lastSliceTime < sliceCooldown)
        {
            isInTarget = false;
            return;
        }

        // Calculate slice data
        Vector3 exitTipPosition = bladeTip.position;
        Vector3 exitBasePosition = bladeBase.position;

        // Calculate cut direction (how the sword moved)
        Vector3 cutDirection = (exitTipPosition - enterTipPosition).normalized;

        // Calculate blade direction (orientation of the blade)
        Vector3 bladeDirection = (enterTipPosition - enterBasePosition).normalized;

        // Calculate cut point (where the blade entered)
        Vector3 cutPoint = (enterTipPosition + enterBasePosition) / 2f;

        // Get the MeshSlicer component from the target
        MeshSlicer slicer = currentTarget.GetComponent<MeshSlicer>();
        if (slicer == null)
        {
            Debug.LogWarning("Target has no MeshSlicer component!");
            isInTarget = false;
            return;
        }

        // Perform the slice with direction
        bool sliced = slicer.SliceObjectWithDirection(currentTarget, cutPoint, cutDirection, bladeDirection);

        if (sliced)
        {
            lastSliceTime = Time.time;
            Debug.Log($"Object sliced! Cut direction: {cutDirection}, Swing speed: {currentSwingSpeed}");

            // Add visual effects here if you have them
            // PlaySliceEffect(cutPoint);
        }

        isInTarget = false;
    }

    // Debug visualization
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isInTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(enterTipPosition, bladeTip.position);
            Gizmos.DrawWireSphere(enterTipPosition, 0.05f);
        }
    }*/
}