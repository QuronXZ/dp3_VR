using UnityEngine;
using ExtraScripts;
public class Sword_script_vr : MonoBehaviour

/*{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;

    private Vector3 lastPos;
    private float swingSpeed;

    void Update()
    {
        // Calculate swing speed
        swingSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only damage if swinging fast enough
        if (swingSpeed < minSwingVelocity) return;

        if (other.TryGetComponent<actor>(out actor enemy))
        {
            enemy.TakeDamage(damage);
            Debug.Log("Hit enemy with swing!");
        }
    }
}
*/
/*
{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;
    public float hitCooldown = 0.2f;

    public sword_fx sword_Fx;

    private Vector3 lastPos;
    private float swingSpeed;
    private float lastHitTime;



    void Update()
    {
        swingSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ⛔ prevent spam hits
        if (Time.time - lastHitTime < hitCooldown) return;

        // ⛔ ignore weak swings
        if (swingSpeed < minSwingVelocity) return;

        // 🎯 check tag
        if (collision.gameObject.CompareTag("hittable"))
        {
            lastHitTime = Time.time;

            // 💥 apply damage
            if (collision.gameObject.TryGetComponent<actor>(out actor enemy))
            {
                enemy.TakeDamage(damage);
                sword_Fx.SpawnVFX(collision);
                sword_Fx.SpawnScratch(collision);
                sword_Fx.PlayHitEffects(collision);
            }

            Debug.Log("Solid hit!");
        }
    }
}*/


/*

{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;
    public float hitCooldown = 0.2f;

    [Header("Slicing Settings")]
    public bool enableSlicing = true;
    public string sliceableTag = "Sliceable"; // Only objects with this tag

    public sword_fx sword_Fx;

    private Vector3 lastPos;
    private float swingSpeed;
    private float lastHitTime;
    private Vector3 lastSwingPoint;

    private MeshSlicer meshSlicer;

    void Start()
    {
        meshSlicer = GetComponent<MeshSlicer>();
        if (meshSlicer == null && enableSlicing)
        {
            meshSlicer = gameObject.AddComponent<MeshSlicer>();
        }
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        swingSpeed = (currentPos - lastPos).magnitude / Time.deltaTime;

        if (swingSpeed > minSwingVelocity)
        {
            lastSwingPoint = currentPos;
        }

        lastPos = currentPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastHitTime < hitCooldown) return;
        if (swingSpeed < minSwingVelocity) return;

        GameObject hitObject = collision.gameObject;

        // ONLY slice objects with the "Sliceable" tag
        if (enableSlicing && meshSlicer != null && hitObject.CompareTag(sliceableTag))
        {
            Vector3 sliceDirection = (transform.position - lastPos).normalized;
            Vector3 slicePoint = collision.contacts[0].point;

            bool sliced = meshSlicer.SliceObject(hitObject, slicePoint, sliceDirection);

            if (sliced)
            {
                lastHitTime = Time.time;
                Debug.Log("Sliceable object sliced!");

                if (sword_Fx != null)
                {
                    sword_Fx.PlayHitEffects(collision);
                }
                return;
            }
        }

        // Regular damage for "hittable" objects (non-sliceable)
        if (hitObject.CompareTag("hittable"))
        {
            lastHitTime = Time.time;

            if (hitObject.TryGetComponent<actor>(out actor enemy))
            {
                enemy.TakeDamage(damage);

                if (sword_Fx != null)
                {
                    sword_Fx.SpawnVFX(collision);
                    sword_Fx.SpawnScratch(collision);
                    sword_Fx.PlayHitEffects(collision);
                }

                Debug.Log("Hit enemy!");
            }
        }
    }
}*/






{
    public int damage = 1;
    public float minSwingVelocity = 1.5f;
    public float hitCooldown = 0.2f;

    [Header("Slicing Settings")]
    public bool enableSlicing = true;
    public string sliceableTag = "Sliceable";

    [Header("Blade References (for directional slicing)")]
    public Transform bladeTip;
    public Transform bladeBase;

    [Header("Slice Effects")]
    public Material sliceMaterial;
    public GameObject sparkParticlesPrefab;
    public float forceAppliedToCut = 5f;

    public sword_fx sword_Fx;

    // Tracking for slice direction
    private Vector3 lastPos;
    private float swingSpeed;
    private float lastHitTime;

    // Blade tracking for accurate slicing
    private Vector3 enterTipPosition;
    private Vector3 enterBasePosition;
    private Vector3 exitTipPosition;
    private Vector3 exitBasePosition;
    private GameObject currentSliceTarget;
    private bool isInSliceable = false;
    private bool hasExited = false;

    void Start()
    {
        // Auto-find blade references if not assigned
        if (bladeTip == null || bladeBase == null)
        {
            Debug.LogWarning("Blade tip or base not assigned! Slicing direction may be inaccurate.");
        }
    }

    void Update()
    {
        // Calculate swing speed
        Vector3 currentPos = transform.position;
        swingSpeed = (currentPos - lastPos).magnitude / Time.deltaTime;
        lastPos = currentPos;
    }

    /*    private void OnTriggerEnter(Collider other)
        {
            if (!enableSlicing) return;
            if (!other.CompareTag(sliceableTag)) return;

            // Store entry positions for directional slicing
            if (bladeTip != null && bladeBase != null)
            {
                enterTipPosition = bladeTip.position;
                enterBasePosition = bladeBase.position;
            }
            else
            {
                // Fallback if blade references missing
                enterTipPosition = transform.position;
                enterBasePosition = transform.position + Vector3.up;
            }

            currentSliceTarget = other.gameObject;
            isInSliceable = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!enableSlicing || !isInSliceable) return;
            if (other.gameObject != currentSliceTarget) return;
            if (!other.CompareTag(sliceableTag)) return;

            // Check if swing is fast enough
            if (swingSpeed < minSwingVelocity)
            {
                Debug.Log($"Swing too slow: {swingSpeed} < {minSwingVelocity}");
                isInSliceable = false;
                return;
            }

            if (Time.time - lastHitTime < hitCooldown)
            {
                isInSliceable = false;
                return;
            }

            // Perform directional slice
            PerformDirectionalSlice(other.gameObject);

            isInSliceable = false;
            currentSliceTarget = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject hitObject = collision.gameObject;

            // Handle regular enemies (hittable)
            if (hitObject.CompareTag("hittable"))
            {
                if (Time.time - lastHitTime < hitCooldown) return;
                if (swingSpeed < minSwingVelocity) return;

                lastHitTime = Time.time;

                if (hitObject.TryGetComponent<actor>(out actor enemy))
                {
                    enemy.TakeDamage(damage);

                    if (sword_Fx != null)
                    {
                        sword_Fx.SpawnVFX(collision);
                        sword_Fx.SpawnScratch(collision);
                        sword_Fx.PlayHitEffects(collision);
                    }

                    Debug.Log("Hit enemy!");
                }
            }
        }*/



    /*
        private void OnCollisionEnter(Collision collision)
        {
            GameObject hitObject = collision.gameObject;

            // Handle sliceable objects (with collision instead of trigger)
            if (enableSlicing && hitObject.CompareTag(sliceableTag))
            {
                if (Time.time - lastHitTime < hitCooldown) return;
                if (swingSpeed < minSwingVelocity) return;

                // Store entry positions for directional slicing
                if (bladeTip != null && bladeBase != null)
                {
                    enterTipPosition = bladeTip.position;
                    enterBasePosition = bladeBase.position;
                }
                else
                {
                    // Fallback if blade references missing
                    enterTipPosition = transform.position;
                    enterBasePosition = transform.position + Vector3.up;
                }

                currentSliceTarget = hitObject; 
                isInSliceable = true;
                hasExited = false;

                // Get exit positions (using the collision point)
                Vector3 exitTipPosition = bladeTip != null ? bladeTip.position : transform.position;
                Vector3 exitBasePosition = bladeBase != null ? bladeBase.position : transform.position + Vector3.up;

                // Calculate swing direction
                Vector3 tipMovement = exitTipPosition - enterTipPosition;
                Vector3 baseMovement = exitBasePosition - enterBasePosition;
                Vector3 swingDirection = (tipMovement + baseMovement).normalized / 2f;

                // Calculate blade orientation
                Vector3 bladeDirection = (enterTipPosition - enterBasePosition).normalized;

                // The cut plane normal is perpendicular to both
                Vector3 cutNormal = Vector3.Cross(swingDirection, bladeDirection).normalized;

                if (cutNormal == Vector3.zero)
                {
                    cutNormal = tipMovement.normalized;
                    if (cutNormal == Vector3.zero) cutNormal = Vector3.up;
                }

                // Cut point is the collision contact point
                Vector3 cutPoint = collision.contacts[0].point;

                // Perform the slice
                PerformDirectionalSlice(hitObject, cutPoint, cutNormal);

                lastHitTime = Time.time;
                return;
            }

            // Handle regular enemies (hittable)
            if (hitObject.CompareTag("hittable"))
            {
                if (Time.time - lastHitTime < hitCooldown) return;
                if (swingSpeed < minSwingVelocity) return;

                lastHitTime = Time.time;

                if (hitObject.TryGetComponent<actor>(out actor enemy))
                {
                    enemy.TakeDamage(damage);

                    if (sword_Fx != null)
                    {
                        sword_Fx.SpawnVFX(collision);
                        sword_Fx.SpawnScratch(collision);
                        sword_Fx.PlayHitEffects(collision);
                    }

                    Debug.Log("Hit enemy!");
                }
            }
        }*/



    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        // Handle sliceable objects - record entry position
        if (enableSlicing && hitObject.CompareTag(sliceableTag))
        {
            if (Time.time - lastHitTime < hitCooldown) return;
            if (swingSpeed < minSwingVelocity) return;

            // Store entry positions
            if (bladeTip != null && bladeBase != null)
            {
                enterTipPosition = bladeTip.position;
                enterBasePosition = bladeBase.position;
            }
            else
            {
                enterTipPosition = transform.position;
                enterBasePosition = transform.position + Vector3.up;
            }

            currentSliceTarget = hitObject;
            isInSliceable = true;
            hasExited = false;

            if (sword_Fx != null)
            {
                sword_Fx.SpawnVFX(collision);
                sword_Fx.Spawnblood(collision);
                sword_Fx.PlayHitEffects(collision);
            }

            Debug.Log($"Entered sliceable object at speed: {swingSpeed}");
        }

        // Handle regular enemies (hittable)
        if (hitObject.CompareTag("hittable"))
        {
            if (Time.time - lastHitTime < hitCooldown) return;
            if (swingSpeed < minSwingVelocity) return;

            lastHitTime = Time.time;

            if (hitObject.TryGetComponent<actor>(out actor enemy))
            {
                enemy.TakeDamage(damage);

                if (sword_Fx != null)
                {
                    sword_Fx.SpawnVFX(collision);
                    sword_Fx.SpawnScratch(collision);
                    sword_Fx.PlayHitEffects(collision);
                }

                Debug.Log("Hit enemy!");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        // Only process sliceable objects we're tracking
        if (!enableSlicing || !isInSliceable) return;
        if (hitObject != currentSliceTarget) return;
        if (!hitObject.CompareTag(sliceableTag)) return;
        if (hasExited) return;

        // Store exit positions
        if (bladeTip != null && bladeBase != null)
        {
            exitTipPosition = bladeTip.position;
            exitBasePosition = bladeBase.position;
        }
        else
        {
            exitTipPosition = transform.position;
            exitBasePosition = transform.position + Vector3.up;
        }

        hasExited = true;

        // Perform directional slice using entry and exit positions
        PerformDirectionalSlice(currentSliceTarget);

        isInSliceable = false;
        currentSliceTarget = null;
        lastHitTime = Time.time;
    }



    private void PerformDirectionalSlice(GameObject target)
    {
        if (bladeTip == null || bladeBase == null)
        {
            Debug.LogError("Blade tip and base must be assigned for directional slicing!");
            return;
        }

        // Calculate swing direction (how the blade moved through the object)
        Vector3 tipMovement = exitTipPosition - enterTipPosition;
        Vector3 baseMovement = exitBasePosition - enterBasePosition;
        Vector3 swingDirection = (tipMovement + baseMovement).normalized / 2f;

        // Calculate blade orientation during cut
        Vector3 bladeDirection = (enterTipPosition - enterBasePosition).normalized;

        // The cut plane normal is perpendicular to both swing direction and blade direction
        Vector3 cutNormal = Vector3.Cross(swingDirection, bladeDirection).normalized;

        // Fallback if cross product is zero
        if (cutNormal == Vector3.zero)
        {
            cutNormal = tipMovement.normalized;
            if (cutNormal == Vector3.zero) cutNormal = Vector3.up;
        }

        // Cut point is the midpoint of the blade at entry or exit
        Vector3 cutPoint = (enterTipPosition + exitTipPosition) / 2f;

        Debug.Log($"SLICE DIRECTION: Swing={swingDirection}, Blade={bladeDirection}, Normal={cutNormal}");

        // Transform to object's local space
        Vector3 localCutPoint = target.transform.InverseTransformPoint(cutPoint);
        Vector3 localCutNormal = target.transform.InverseTransformDirection(cutNormal).normalized;

        // Create the slicing plane
        Plane slicePlane = new Plane(localCutNormal, localCutPoint);

        // Ensure consistent orientation
        if (Vector3.Dot(localCutNormal, Vector3.up) < 0)
        {
            slicePlane = new Plane(-localCutNormal, localCutPoint);
            cutNormal = -cutNormal;
        }

        // Perform the slice
        GameObject[] slices = Slicer.Slice(slicePlane, target, sliceMaterial, ref sparkParticlesPrefab);

        if (slices == null || slices.Length < 2)
        {
            Debug.LogWarning("Slice failed!");
            return;
        }

        // Apply forces and setup auto-destroy
        for (int i = 0; i < slices.Length; i++)
        {
            if (slices[i] != null)
            {
                Rigidbody rb = slices[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 directionToPiece = (slices[i].transform.position - cutPoint).normalized;
                    Vector3 forceDirection = (cutNormal + directionToPiece * 0.5f).normalized;
                    rb.AddForce(forceDirection * forceAppliedToCut, ForceMode.Impulse);
                }

                // Auto-destroy after 5 seconds
                Destroy(slices[i], 5f);
            }
        }

        // Destroy the original object
        Destroy(target);

        Debug.Log($"Directional slice successful! Cut normal: {cutNormal}");
    }
    
    
    /*
        private void PerformDirectionalSlice(GameObject target, Vector3 cutPoint, Vector3 cutNormal)
        {
            if (bladeTip == null || bladeBase == null)
            {
                Debug.LogError("Blade tip and base must be assigned for directional slicing!");
                return;
            }

            // Transform to object's local space
            Vector3 localCutPoint = target.transform.InverseTransformPoint(cutPoint);
            Vector3 localCutNormal = target.transform.InverseTransformDirection(cutNormal).normalized;

            // Create the slicing plane
            Plane slicePlane = new Plane(localCutNormal, localCutPoint);

            // Ensure consistent orientation
            if (Vector3.Dot(localCutNormal, Vector3.up) < 0)
            {
                slicePlane = new Plane(-localCutNormal, localCutPoint);
                cutNormal = -cutNormal;
            }

            Debug.Log($"SLICE: Speed={swingSpeed}, Normal={cutNormal}, Point={cutPoint}");

            // Perform the slice
            GameObject[] slices = Slicer.Slice(slicePlane, target, sliceMaterial, ref sparkParticlesPrefab);

            if (slices == null || slices.Length < 2)
            {
                Debug.LogWarning("Slice failed!");
                return;
            }

            // Apply forces and setup auto-destroy
            for (int i = 0; i < slices.Length; i++)
            {
                if (slices[i] != null)
                {
                    Rigidbody rb = slices[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 directionToPiece = (slices[i].transform.position - cutPoint).normalized;
                        Vector3 forceDirection = (cutNormal + directionToPiece * 0.5f).normalized;
                        rb.AddForce(forceDirection * forceAppliedToCut, ForceMode.Impulse);
                    }

                    // Auto-destroy after 5 seconds
                    Destroy(slices[i], 5f);
                }
            }

            // Destroy the original object
            Destroy(target);

            Debug.Log("Directional slice successful! Pieces will disappear after 5 seconds.");
        }*/

    /*   private void PerformDirectionalSlice(GameObject target)
       {
           if (bladeTip == null || bladeBase == null)
           {
               Debug.LogError("Blade tip and base must be assigned for directional slicing!");
               return;
           }

           // Get exit positions
           Vector3 exitTipPosition = bladeTip.position;
           Vector3 exitBasePosition = bladeBase.position;

           // Calculate swing direction (how the blade moved through the object)
           Vector3 tipMovement = exitTipPosition - enterTipPosition;
           Vector3 baseMovement = exitBasePosition - enterBasePosition;
           Vector3 swingDirection = (tipMovement + baseMovement).normalized / 2f;

           // Calculate blade orientation (from base to tip)
           Vector3 bladeDirection = (enterTipPosition - enterBasePosition).normalized;

           // The cut plane normal is perpendicular to both swing direction and blade direction
           Vector3 cutNormal = Vector3.Cross(swingDirection, bladeDirection).normalized;

           // Fallback if cross product is zero
           if (cutNormal == Vector3.zero)
           {
               cutNormal = tipMovement.normalized;
               if (cutNormal == Vector3.zero) cutNormal = Vector3.up;
           }

           // Cut point is the midpoint of the blade at entry
           Vector3 cutPoint = (enterTipPosition + enterBasePosition) / 2f;

           // Transform to object's local space
           Vector3 localCutPoint = target.transform.InverseTransformPoint(cutPoint);
           Vector3 localCutNormal = target.transform.InverseTransformDirection(cutNormal).normalized;

           // Create the slicing plane
           Plane slicePlane = new Plane(localCutNormal, localCutPoint);

           // Ensure consistent orientation
           if (Vector3.Dot(localCutNormal, Vector3.up) < 0)
           {
               slicePlane = new Plane(-localCutNormal, localCutPoint);
               cutNormal = -cutNormal;
           }

           Debug.Log($"SLICE: Speed={swingSpeed}, Direction={swingDirection}, Normal={cutNormal}");

           // Perform the slice using your Slicer class
           GameObject[] slices = Slicer.Slice(slicePlane, target, sliceMaterial, ref sparkParticlesPrefab);

           if (slices == null || slices.Length < 2)
           {
               Debug.LogWarning("Slice failed!");
               return;
           }

           lastHitTime = Time.time;

           // Apply forces to the sliced pieces
           for (int i = 0; i < slices.Length; i++)
           {
               if (slices[i] != null)
               {
                   Rigidbody rb = slices[i].GetComponent<Rigidbody>();
                   if (rb != null)
                   {
                       Vector3 directionToPiece = (slices[i].transform.position - cutPoint).normalized;
                       Vector3 forceDirection = (cutNormal + directionToPiece * 0.5f).normalized;
                       rb.AddForce(forceDirection * forceAppliedToCut, ForceMode.Impulse);
                   }
               }
           }

           // Play effects
           if (sword_Fx != null)
           {
               // Create a fake contact at cut point for effects
               // sword_Fx.PlayHitEffects at cutPoint
           }

           // Destroy original object (Slicer.Slice already destroys? Check)
           // Destroy(target);

           Debug.Log("Directional slice successful!");
       }*/

    /*   private void PerformDirectionalSlice(GameObject target)
       {
           if (bladeTip == null || bladeBase == null)
           {
               Debug.LogError("Blade tip and base must be assigned for directional slicing!");
               return;
           }

           // Get exit positions
           Vector3 exitTipPosition = bladeTip.position;
           Vector3 exitBasePosition = bladeBase.position;

           // Calculate swing direction (how the blade moved through the object)
           Vector3 tipMovement = exitTipPosition - enterTipPosition;
           Vector3 baseMovement = exitBasePosition - enterBasePosition;
           Vector3 swingDirection = (tipMovement + baseMovement).normalized / 2f;

           // Calculate blade orientation (from base to tip)
           Vector3 bladeDirection = (enterTipPosition - enterBasePosition).normalized;

           // The cut plane normal is perpendicular to both swing direction and blade direction
           Vector3 cutNormal = Vector3.Cross(swingDirection, bladeDirection).normalized;

           // Fallback if cross product is zero
           if (cutNormal == Vector3.zero)
           {
               cutNormal = tipMovement.normalized;
               if (cutNormal == Vector3.zero) cutNormal = Vector3.up;
           }

           // Cut point is the midpoint of the blade at entry
           Vector3 cutPoint = (enterTipPosition + enterBasePosition) / 2f;

           // Transform to object's local space
           Vector3 localCutPoint = target.transform.InverseTransformPoint(cutPoint);
           Vector3 localCutNormal = target.transform.InverseTransformDirection(cutNormal).normalized;

           // Create the slicing plane
           Plane slicePlane = new Plane(localCutNormal, localCutPoint);

           // Ensure consistent orientation
           if (Vector3.Dot(localCutNormal, Vector3.up) < 0)
           {
               slicePlane = new Plane(-localCutNormal, localCutPoint);
               cutNormal = -cutNormal;
           }

           Debug.Log($"SLICE: Speed={swingSpeed}, Direction={swingDirection}, Normal={cutNormal}");

           // Perform the slice using your Slicer class
           GameObject[] slices = Slicer.Slice(slicePlane, target, sliceMaterial, ref sparkParticlesPrefab);

           if (slices == null || slices.Length < 2)
           {
               Debug.LogWarning("Slice failed!");
               return;
           }

           lastHitTime = Time.time;

           // Apply forces to the sliced pieces and setup auto-destroy
           for (int i = 0; i < slices.Length; i++)
           {
               if (slices[i] != null)
               {
                   // Apply force to make pieces fly apart
                   Rigidbody rb = slices[i].GetComponent<Rigidbody>();
                   if (rb != null)
                   {
                       Vector3 directionToPiece = (slices[i].transform.position - cutPoint).normalized;
                       Vector3 forceDirection = (cutNormal + directionToPiece * 0.5f).normalized;
                       rb.AddForce(forceDirection * forceAppliedToCut, ForceMode.Impulse);
                   }

                   // Add auto-destroy after 5 seconds
                   Destroy(slices[i], 5f);
               }
           }

           // Destroy the original object (no copies left behind)
           Destroy(target);

           Debug.Log("Directional slice successful! Pieces will disappear after 5 seconds.");
       }
   */
    // Debug visualization
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isInSliceable && bladeTip != null && bladeBase != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(enterTipPosition, bladeTip.position);
            Gizmos.DrawWireSphere(enterTipPosition, 0.03f);
            Gizmos.DrawWireSphere(enterBasePosition, 0.03f);
        }
    }
}