
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LockedDoor : MonoBehaviour
{
    public string sceneToLoad = "Level2";
    public string spawnPointName;
    public TextMeshProUGUI interactionText;
    public float interactionDistance = 2.0f;

    private QuestTracker questTracker;
    private Transform player;
    private Collider2D doorCollider;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        questTracker = FindObjectOfType<QuestTracker>();
        doorCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (player == null || interactionText == null)
            return;

        float distance = Vector2.Distance(
            player.position,
            transform.position
        );

        if (distance > interactionDistance)
            return;

        if (questTracker == null)
            questTracker = FindObjectOfType<QuestTracker>();

        if (questTracker == null)
            return;

        if (!questTracker.hasKey)
        {
            interactionText.text = "Find the key first!";
            return;
        }

        interactionText.text = "Press F to unlock the door";

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (doorCollider != null)
                doorCollider.enabled = false;

            PLAYER.spawnPointName = spawnPointName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
