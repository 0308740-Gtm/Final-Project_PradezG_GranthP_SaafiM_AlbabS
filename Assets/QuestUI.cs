using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    private QuestTracker questTracker; //Stores QuestTracker script so the UI can access quest state

    private TextMeshProUGUI interactionText; //Stores the text component on screen

    private string temporaryMessage = ""; //Stores temporary message such as a pickup instruction

    private float temporaryMessageTime = 0f; //Stores how long the message should stay on screen for


    void Start()
    {
        interactionText = GetComponent<TextMeshProUGUI>(); //Gets the TextMeshPro Component attached to the gameobject

        questTracker = FindObjectOfType<QuestTracker>(); //Finds the existing QuestTracker so the UI can read the quest state
    }


    void Update()
    {
        if (temporaryMessageTime > 0f)
        {
            temporaryMessageTime -= Time.deltaTime;
            interactionText.text = temporaryMessage;
            return;
        }

        if (questTracker == null)
        {
            return;
        }

        if (questTracker.questHandedIn)
        {
            interactionText.text =
                "Quest complete! Thank you for returning the key.";
        }
        else if (questTracker.questCompleted)
        {
            interactionText.text =
                "Key found! Return to the Merchant.";
        }
        else if (questTracker.questAccepted)
        {
            interactionText.text =
                "Quest in progress: Find the key in the temple.";
        }
        else
        {
            interactionText.text = "Go to the Merchant";
        }
    }


    public void ShowMessage(string message, float duration)
    {
        temporaryMessage = message; //Stores message to be temporarly shown

        temporaryMessageTime = duration; //Stores time temporary message to be shown for
    }

    public void HideText() //Hides the UI text
    {
        interactionText.enabled = false;
    }

    public void ShowText() //Makes the UI text visible again
    {
        interactionText.enabled = true;
    }
}

