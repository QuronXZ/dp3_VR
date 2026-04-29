using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTutorialPath : MonoBehaviour
{
    [Header("Path Points (in order)")]
    public List<Transform> pathPoints;

    [Header("Settings")]
    public float detectionRadius = 0.3f;
    public float maxTime = 3f;

    private int currentIndex = 0;
    private float timer;
    private bool started = false;

    public Transform swordTip;

    void Update()
    {
        if (swordTip == null || pathPoints.Count == 0) return;

        // Start when first point is hit
        if (!started)
        {
            if (IsNearPoint(pathPoints[0]))
            {
                started = true;
                timer = maxTime;
                currentIndex = 0;

                CutPoint(currentIndex); // cut first point
                currentIndex++;

                Debug.Log("Started Path");
            }
            return;
        }

        // Timer countdown
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Fail();
            return;
        }

        // Check next point
        if (currentIndex < pathPoints.Count && IsNearPoint(pathPoints[currentIndex]))
        {
            CutPoint(currentIndex);

            currentIndex++;

            if (currentIndex >= pathPoints.Count)
            {
                Success();
            }
        }
    }

    bool IsNearPoint(Transform point)
    {
        return Vector3.Distance(swordTip.position, point.position) <= detectionRadius;
    }

    void CutPoint(int index)
    {
        if (pathPoints[index] != null)
        {
            Debug.Log("Obj " + (index + 1) + " cut");

            // Hide object (instead of destroying for safety)
            pathPoints[index].gameObject.SetActive(false);
        }
    }
    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPath();
    }

    void Success()
    {

        Debug.Log("SUCCESS");
        StartCoroutine(ResetAfterDelay(3f)); // delay reset
        ResetPath();
    }

    void Fail()
    {
        Debug.Log("FAIL");
        ResetPath();
    }

    void ResetPath()
    {
        started = false;
        currentIndex = 0;

        // Reactivate all points for retry
        foreach (Transform t in pathPoints)
        {
            if (t != null)
                t.gameObject.SetActive(true);
        }
    }
}