
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void Start()
    {
        // Make sure the game is not paused
        Time.timeScale = 1f;

        // Remove the persistent player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }

        // Remove the persistent quest tracker
        QuestTracker tracker = FindObjectOfType<QuestTracker>();

        if (tracker != null)
        {
            Destroy(tracker.gameObject);
        }
    }

    public void ReturnHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LandingPage");
    }
}
