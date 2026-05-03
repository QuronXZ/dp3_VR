using UnityEngine;
using System.Collections.Generic;

public class MeshSlicer : MonoBehaviour
{
    [Header("Slicing Settings")]
    public float sliceForce = 5f;
    public Material slicedMaterial;
    public bool addRigidbodies = true;

    private HashSet<GameObject> slicedObjects = new HashSet<GameObject>();

    // NEW: Method that accepts cut direction and point
    // Add this method to your MeshSlicer class
/*    public bool SliceObjectWithDirection(GameObject target, Vector3 cutPoint, Vector3 sliceNormal, Vector3 cutDirection)
    {
        if (slicedObjects.Contains(target)) return false;

        MeshFilter meshFilter = target.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();

        if (meshFilter == null || meshRenderer == null)
        {
            Debug.LogWarning($"{target.name} has no MeshFilter or MeshRenderer - can't slice");
            return false;
        }

        // Transform cut point and normal to object's local space
        Vector3 localCutPoint = target.transform.InverseTransformPoint(cutPoint);
        Vector3 localSliceNormal = target.transform.InverseTransformDirection(sliceNormal).normalized;

        // Create the slicing plane
        Plane slicePlane = new Plane(localSliceNormal, localCutPoint);

        // Ensure consistent orientation
        if (Vector3.Dot(localSliceNormal, Vector3.up) < 0)
        {
            slicePlane = new Plane(-localSliceNormal, localCutPoint);
        }

        // Perform the actual slice (using your existing slicing logic)
        GameObject[] slices = PerformActualSlice(slicePlane, target);

        if (slices != null && slices.Length >= 2)
        {
            slicedObjects.Add(target);

            // Apply forces in cut direction
            foreach (GameObject slice in slices)
            {
                if (slice != null)
                {
                    Rigidbody rb = slice.GetComponent<Rigidbody>();
                    if (rb != null && addRigidbodies)
                    {
                        rb.AddForce(cutDirection * sliceForce, ForceMode.Impulse);
                    }
                }
            }

            Destroy(target);
            return true;
        }

        return false;
    }

    // You need to implement this based on your slicing method
    private GameObject[] PerformSlice(Plane plane, GameObject target)
    {
        // Option 1: If you have a Slicer class (like in SwordSlice)
        // return Slicer.Slice(plane, target, slicedMaterial, ref someParticlePrefab);

        // Option 2: Simple slice for now (creates two halves)
        return CreateSimpleSlice(target, plane);
    }

    private GameObject[] CreateSimpleSlice(GameObject original, Plane plane)
    {
        // Simple implementation - creates two halves based on plane
        GameObject slice1 = Instantiate(original, original.transform.position, original.transform.rotation);
        GameObject slice2 = Instantiate(original, original.transform.position, original.transform.rotation);

        slice1.name = original.name + "_Slice1";
        slice2.name = original.name + "_Slice2";

        // Add rigidbodies
        if (addRigidbodies)
        {
            if (slice1.GetComponent<Rigidbody>() == null) slice1.AddComponent<Rigidbody>();
            if (slice2.GetComponent<Rigidbody>() == null) slice2.AddComponent<Rigidbody>();
        }

        return new GameObject[] { slice1, slice2 };
    }

    public void ResetSlicedObjects()
    {
        slicedObjects.Clear();
    }*/
}