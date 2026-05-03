using UnityEngine;


public class TriggerZone : MonoBehaviour
{
    public int cardIndexToShow = 0;
    private bool triggered = false;
    public GameManager_lvl1 lvl1;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        lvl1.ShowCardFromTrigger(cardIndexToShow);
    }
}