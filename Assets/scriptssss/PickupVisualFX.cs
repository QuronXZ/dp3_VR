
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class PickupVisualFX : MonoBehaviour
{
    [Header("Snap Settings")]
    [SerializeField] private Transform snapTarget;      // Where object snaps to
    [SerializeField] private float snapSpeed = 15f;     // How fast it snaps back
    [SerializeField] private bool snapOnStart = true;   // Snap at game start

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private Rigidbody rb;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void Start()
    {
        if (snapOnStart && snapTarget != null)
        {
            SnapImmediately();
        }
    }

    void Update()
    {
        // Smoothly move back when not grabbed
        if (!isGrabbed && snapTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, snapTarget.position, Time.deltaTime * snapSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, snapTarget.rotation, Time.deltaTime * snapSpeed);

            // Stop physics movement once close enough
            if (Vector3.Distance(transform.position, snapTarget.position) < 0.01f && rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Optionally unfreeze rigidbody constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    public void SnapImmediately()
    {
        if (snapTarget != null)
        {
            transform.position = snapTarget.position;
            transform.rotation = snapTarget.rotation;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    void OnDestroy()
    {
        // Clean up listeners
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}