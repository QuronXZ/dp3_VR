using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTutorialPath : MonoBehaviour
{
    [Header("Path Points (in order)")]
    public List<Transform> pathPoints1, pathPoints2, pathPoints3;
    private List <Transform> pathPoints;

    [Header("Settings")]
    public float detectionRadius = 0.3f;
    public float maxTime = 3f;

    private int currentIndex = 0;
    private float timer;
    private bool started = false;

    public Transform swordTip;
    public GameManager_lvl1 manager;
    public CardParentManager cardParentManager;


    private void Start()
    {
        pathPoints = pathPoints1;
    }
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
        if (pathPoints == pathPoints1)
        {
            Debug.Log("its 1st list");
            pathPoints.Clear();
            pathPoints = pathPoints2;
            cardParentManager.ForceShowCard(6);
        }
        else if (pathPoints == pathPoints2)
        {
            Debug.Log("its 2st list");
            pathPoints.Clear();
            pathPoints = pathPoints3;
            cardParentManager.ForceShowCard(7);
        }
        else if (pathPoints == pathPoints3)
        {
            Debug.Log("its 3rd list");
            manager.EnemyDied(pathPoints[0].gameObject);
        }

        
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
/*
public class SwordTutorialPath : MonoBehaviour
{
    [Header("Tutorial Stages")]
    public List<Stage> stages;

    [Header("Settings")]
    public float detectionRadius = 0.3f;
    public float maxTime = 3f;

    [Header("References")]
    public Transform swordTip;
    public GameManager_lvl1 manager;
    public CardParentManager cardManager;

    private int currentStageIndex = 0;
    private int currentPointIndex = 0;
    private float timer;
    private bool stageActive = false;
    private bool allStagesComplete = false;

    [System.Serializable]
    public class Stage
    {
        public string stageName;
        public List<Transform> pathPoints;
        public int cardIndexToShow; // Card to show after completing this stage
    }

    void Start()
    {
        if (stages.Count > 0)
        {
            StartStage(0);
        }
    }

    void Update()
    {
        if (allStagesComplete) return;
        if (!stageActive) return;
        if (swordTip == null) return;

        Stage currentStage = stages[currentStageIndex];

        // Start stage when first point is hit
        if (currentPointIndex == 0)
        {
            if (IsNearPoint(currentStage.pathPoints[0]))
            {
                stageActive = true;
                timer = maxTime;

                CutPoint(currentStage.pathPoints[0]);
                currentPointIndex++;

                Debug.Log($"Started Stage: {currentStage.stageName}");
            }
            return;
        }

        // Timer countdown
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            FailStage();
            return;
        }

        // Check next point
        if (currentPointIndex < currentStage.pathPoints.Count &&
            IsNearPoint(currentStage.pathPoints[currentPointIndex]))
        {
            CutPoint(currentStage.pathPoints[currentPointIndex]);
            currentPointIndex++;

            if (currentPointIndex >= currentStage.pathPoints.Count)
            {
                SuccessStage();
            }
        }
    }

    bool IsNearPoint(Transform point)
    {
        return Vector3.Distance(swordTip.position, point.position) <= detectionRadius;
    }

    void CutPoint(Transform point)
    {
        if (point != null)
        {
            Debug.Log($"Cut: {point.name}");
            point.gameObject.SetActive(false);
        }
    }

    void SuccessStage()
    {
        Stage completedStage = stages[currentStageIndex];
        Debug.Log($"SUCCESS! Completed: {completedStage.stageName}");

        stageActive = false;

        // Show the card for this stage
        if (cardManager != null && completedStage.cardIndexToShow >= 0)
        {
            cardManager.ForceShowCard(completedStage.cardIndexToShow);
        }

        // Move to next stage
        currentStageIndex++;

        if (currentStageIndex < stages.Count)
        {
            // Start next stage after card is shown
            StartCoroutine(StartNextStageAfterDelay(2f));
        }
        else
        {
            // All stages complete!
            AllStagesComplete();
        }
    }

    IEnumerator StartNextStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartStage(currentStageIndex);
    }

    void StartStage(int stageIndex)
    {
        Stage stage = stages[stageIndex];

        // Reactivate all points for this stage
        foreach (Transform point in stage.pathPoints)
        {
            if (point != null)
                point.gameObject.SetActive(true);
        }

        currentPointIndex = 0;
        stageActive = true;
        timer = maxTime;

        Debug.Log($"Starting Stage: {stage.stageName}");
    }

    void FailStage()
    {
        Debug.Log($"FAIL! Reset Stage: {stages[currentStageIndex].stageName}");
        ResetCurrentStage();
    }

    void ResetCurrentStage()
    {
        Stage currentStage = stages[currentStageIndex];

        // Reactivate all points for current stage
        foreach (Transform point in currentStage.pathPoints)
        {
            if (point != null)
                point.gameObject.SetActive(true);
        }

        currentPointIndex = 0;
        stageActive = true;
        timer = maxTime;
    }

    void AllStagesComplete()
    {
        allStagesComplete = true;
        stageActive = false;
        Debug.Log("ALL TUTORIAL STAGES COMPLETE! Unlocking gate...");

        // Unlock gate
        if (manager != null)
        {
            // Call method to open gate
            //manager.UnlockGateFromTutorial();
            //manager.EnemyDied(pathPoints[0].gameObject);
        }
    }
}*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTutorialPath : MonoBehaviour
{
    [Header("Tutorial Lists")]
    public List<Transform> tutorialList1; // First set of balls
    public List<Transform> tutorialList2; // Second set of balls
    public List<Transform> tutorialList3; // Third set of balls

    [Header("Settings")]
    public float detectionRadius = 0.3f;
    public float maxTime = 3f;

    [Header("References")]
    public Transform swordTip;
    public GameManager_lvl2 manager;

    private int currentListIndex = 0; // 0=List1, 1=List2, 2=List3
    private int currentPointIndex = 0;
    private float timer;
    private bool tutorialActive = false;
    private List<Transform> currentList;

    void Start()
    {
        // Start with first list
        StartTutorialList(0);
    }

    void Update()
    {
        if (!tutorialActive) return;
        if (swordTip == null) return;

        // Start when first point is hit
        if (currentPointIndex == 0)
        {
            if (IsNearPoint(currentList[0]))
            {
                tutorialActive = true;
                timer = maxTime;

                CutPoint(currentList[0]);
                currentPointIndex++;

                Debug.Log($"Started Tutorial List {currentListIndex + 1}");
            }
            return;
        }

        // Timer countdown
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            FailTutorial();
            return;
        }

        // Check next point
        if (currentPointIndex < currentList.Count && IsNearPoint(currentList[currentPointIndex]))
        {
            CutPoint(currentList[currentPointIndex]);
            currentPointIndex++;

            if (currentPointIndex >= currentList.Count)
            {
                SuccessTutorial();
            }
        }
    }

    bool IsNearPoint(Transform point)
    {
        return Vector3.Distance(swordTip.position, point.position) <= detectionRadius;
    }

    void CutPoint(Transform point)
    {
        if (point != null)
        {
            Debug.Log($"Cut: {point.name}");
            point.gameObject.SetActive(false);
        }
    }

    void StartTutorialList(int listIndex)
    {
        if (listIndex == 0) currentList = tutorialList1;
        else if (listIndex == 1) currentList = tutorialList2;
        else currentList = tutorialList3;

        // Reactivate all points in this list
        foreach (Transform point in currentList)
        {
            if (point != null)
                point.gameObject.SetActive(true);
        }

        currentPointIndex = 0;
        tutorialActive = true;
        timer = maxTime;

        Debug.Log($"Starting Tutorial List {listIndex + 1}");
    }

    void SuccessTutorial()
    {
        Debug.Log($"SUCCESS! Completed Tutorial List {currentListIndex + 1}");
        tutorialActive = false;

        // Show card based on which list completed
        if (currentListIndex == 0) // First list done
        {
            manager.ShowCardFromTrigger(6); // Show card 6
            StartCoroutine(WaitForCardAndStartNext(7));
        }
        else if (currentListIndex == 1) // Second list done
        {
            manager.ShowCardFromTrigger(7); // Show card 7
            StartCoroutine(WaitForCardAndStartNext(8));
        }
        else if (currentListIndex == 2) // Third list done
        {
            manager.ShowCardFromTrigger(9); // Show card 8
            StartCoroutine(UnlockGateAfterCard());
        }
    }

    IEnumerator WaitForCardAndStartNext(int cardIndex)
    {
        // Wait for card to be shown and closed
        yield return new WaitUntil(() => manager.IsCardClosed(cardIndex));

        // Move to next tutorial list
        currentListIndex++;
        StartTutorialList(currentListIndex);
    }

    IEnumerator UnlockGateAfterCard()
    {
        // Wait for card to be shown and closed
        yield return new WaitUntil(() => manager.IsCardClosed(8));

        // Unlock the gate
        manager.EnemyDied(tutorialList3[0].gameObject);
    }

    void FailTutorial()
    {
        Debug.Log($"FAIL! Resetting Tutorial List {currentListIndex + 1}");

        // Reset current list only
        foreach (Transform point in currentList)
        {
            if (point != null)
                point.gameObject.SetActive(true);
        }

        currentPointIndex = 0;
        tutorialActive = true;
        timer = maxTime;
    }
}*/