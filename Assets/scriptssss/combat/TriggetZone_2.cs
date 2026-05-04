using UnityEngine;

public class TriggetZone_2 : MonoBehaviour
{
    public int cardIndexToShow = 6;
    private bool triggered = false;
    public GameManager_lvl2 lvl2;

    void OnTriggerExit(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        if (gameObject.name == "entrry1 (1)")
        {
            Debug.Log("INSIDE 2nd lvlllll");
        }
        if (gameObject.name == "entrry1")
        {
            Debug.Log("INSIDE 1ST lvlllll");
        }

        triggered = true;
        lvl2.ShowCardFromTrigger(cardIndexToShow);
    }
}