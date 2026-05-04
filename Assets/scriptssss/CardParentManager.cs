using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*public class CardParentManager : MonoBehaviour
{
    public List<GameObject> cards;
    private int currentCardIndex = 0;
    public Button continueButton; // Assign in Inspector

    private dailogue_appear currentDialogue;

    void Start()
    {
        // Show first card, hide others
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetActive(i == 0);
        }

        // Find the dialogue script on the active card
        FindActiveDialogue();

        // Setup button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinuePressed);
        }

    }
    void FindActiveDialogue()
    {
        // Get the dialogue script from the currently active card
        foreach (GameObject card in cards)
        {
            if (card.activeInHierarchy)
            {
                currentDialogue = card.GetComponent<dailogue_appear>();
                break;
            }
        }
    }


    public void OnContinuePressed()
    {
        if (currentDialogue != null)
        {
            currentDialogue.NextDialogueBtn();
        }
    
    }


    public void OnCardCompleted(GameObject completedCard)
    {
        completedCard.SetActive(false);
        currentCardIndex++;

        if (currentCardIndex < cards.Count)
        {
            cards[currentCardIndex].SetActive(true);
            FindActiveDialogue(); // Update to new card's dialogue
        }
        else
        {
            Debug.Log("All cards completed!");
            if (continueButton != null)
                continueButton.gameObject.SetActive(false);
        }
    }
}*/


using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardParentManager : MonoBehaviour
{
    public List<GameObject> cards;
    public Button continueButton; // Assign in Inspector
    public Canvas canva;

    private int currentCardIndex = 0;

    private dailogue_appear currentDialogue;
    private GameObject currentActiveCard;
    void Start()
    {
        // Show first card, hide others
        for (int i = 0; i < cards.Count; i++)
        {
            canva.enabled = true;
            cards[i].SetActive(i == 0);
        }

        // Find the dialogue script on the active card
        FindActiveDialogue();
        ShowOnlyCard(0);

        // Setup button
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinuePressed);
        }
    }


    // New method to show only one card and hide all others
    void ShowOnlyCard(int cardIndex)
    {
        HideAllCards(); 
        canva.enabled = true;
        cards[cardIndex].SetActive(true);
        currentActiveCard = cards[cardIndex];
        Debug.Log($"Showing card {cardIndex}, hiding all others");
    }


    dailogue_appear FindActiveDialogue()
    {
        // Get the dialogue script from the currently active card
        foreach (GameObject card in cards)
        {
            if (card != null && card.activeInHierarchy)
            {
                // Look for dailogue_appear in the child named "ModalText"
                Transform modalText = card.transform.Find("Modal Text");

                if (modalText != null)
                {
                    dailogue_appear dialogue = modalText.GetComponent<dailogue_appear>();
                    if (dialogue != null)
                        return dialogue;
                }

                // Fallback: try to find anywhere in card if ModalText not found
                dailogue_appear fallbackDialogue = card.GetComponentInChildren<dailogue_appear>();
                if (fallbackDialogue != null)
                    return fallbackDialogue;
            }
        }
        return null;
    }

    void OnContinuePressed()
    {
        dailogue_appear currentDialogue = FindActiveDialogue();
        if (currentDialogue != null)
        {
            currentDialogue.NextDialogueBtn();
        }
        else
        {
            Debug.LogWarning("No active dialogue found!");
        }
    }

    public void OnCardCompleted(GameObject completedCard)
    {
        Debug.Log($"Card completed: {completedCard.name}");
        completedCard.SetActive(false);
        currentCardIndex++;
        ShowOnlyCard(currentCardIndex);
        if (currentCardIndex < 3)
        {

            ShowOnlyCard(currentCardIndex);
            //cards[currentCardIndex].SetActive(true);
            //FindActiveDialogue(); // Update to new card's dialogue
        }
        /*if (currentCardIndex == 3)
        {
            canva.enabled = false;
            HideAllCards();
        }*/
        else
        {
            HideAllCards();
            Debug.Log("All cards completed!");
            if (continueButton != null)
                canva.enabled = false;
        }
    }


    // Optional: Method to hide all cards
    void HideAllCards()
    {
        foreach (GameObject card in cards)
        {
            if (card != null)
                card.SetActive(false);
        }
    }

    // Optional: Method to manually show a specific card from GameManager
    public void ForceShowCard(int cardIndex)
    {
        if (cardIndex >= 0 && cardIndex < cards.Count)
        {

            Debug.Log("card indexxxx: "+ cardIndex); 
            currentCardIndex = cardIndex;
            canva.enabled = true;
            ShowOnlyCard(currentCardIndex);
        }
        else
        {
            Debug.Log("in EKKKEEERRR");
            Debug.Log("in EKKKEEERRR");
        }
    }
}