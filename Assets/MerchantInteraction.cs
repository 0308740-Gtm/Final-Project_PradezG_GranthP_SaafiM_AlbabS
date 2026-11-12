
using UnityEngine;

public class MerchantInteraction : MonoBehaviour
{
    private Transform player;

    public TMPro.TextMeshProUGUI interactionText;
    public PLAYER playerMovement;

    private bool isTalking = false;
    private int dialogueStep = 0;

    private QuestTracker questTracker;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        questTracker = FindObjectOfType<QuestTracker>();
    }

    void Update()
    {
        if (player == null || questTracker == null ||
            interactionText == null || playerMovement == null)
        {
            return;
        }

        float distance = Vector2.Distance(
            transform.position, player.position
        );

        // Handle dialogue first so other messages cannot overwrite it.
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (dialogueStep == 1)
                {
                    dialogueStep = 2;
                    interactionText.text =
                        "Could you search the temple and bring the key back to me?\nPress X to continue";
                }
                else if (dialogueStep == 2)
                {
                    dialogueStep = 3;
                    interactionText.text =
                        "Will you help me find the key?\nPress Y to accept the quest";
                }
                else if (dialogueStep == 4)
                {
                    dialogueStep = 5;
                    interactionText.text =
                        "Wonderful! Thank you for finding it.\nPress X to continue";
                }
                else if (dialogueStep == 5)
                {
                    questTracker.questHandedIn = true;

                    dialogueStep = 6;
                    interactionText.text =
                        "You have completed your quest. Thank you, explorer!\nPress X to finish";
                }
                else if (dialogueStep == 6)
                {
                    isTalking = false;
                    dialogueStep = 0;
                    playerMovement.canMove = true;

                    interactionText.text =
                        "Thank you for your help, explorer.";
                }
            }

            // Accept the initial quest using Y.
            if (isTalking && dialogueStep == 3 &&
                Input.GetKeyDown(KeyCode.Y))
            {
                questTracker.questAccepted = true;

                isTalking = false;
                dialogueStep = 0;
                playerMovement.canMove = true;

                interactionText.text =
                    "Quest accepted! Find the key in the temple.";
            }

            return;
        }

        // The quest has been handed in.
        if (questTracker.questHandedIn)
        {
            if (distance <= 2.8f)
            {
                interactionText.text =
                    "Thank you for returning the key!";
            }
            else
            {
                interactionText.text = "Quest complete!";
            }
        }

        // The key has been found but not returned.
        else if (questTracker.questCompleted)
        {
            if (distance <= 2.8f)
            {
                interactionText.text =
                    "Press X to return the key to the Merchant";

                if (Input.GetKeyDown(KeyCode.X))
                {
                    isTalking = true;
                    playerMovement.canMove = false;
                    dialogueStep = 4;

                    interactionText.text =
                        "Welcome back, explorer! You found the key!\nPress X to continue";
                }
            }
            else
            {
                interactionText.text =
                    "Key found! Return to the Merchant.";
            }
        }

        // The quest has been accepted, but the key has not been found.
        else if (questTracker.questAccepted)
        {
            interactionText.text =
                "Quest in progress: Find the key in the temple.";
        }

        // The player has not accepted the quest yet.
        else
        {
            if (distance <= 2.8f)
            {
                interactionText.text =
                    "Press X to talk to the Merchant";

                if (Input.GetKeyDown(KeyCode.X))
                {
                    isTalking = true;
                    playerMovement.canMove = false;
                    dialogueStep = 1;

                    interactionText.text =
                        "Hey there, explorer. I need your help.\nA key has been lost in the temple.\nPress X to continue";
                }
            }
            else
            {
                interactionText.text = "Go to the Merchant";
            }
        }
    }
}
