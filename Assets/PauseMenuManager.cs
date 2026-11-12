
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;
    private static bool isMuted = false;

    private void Start()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        AudioListener.volume = isMuted ? 0f : 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        QuestTracker questTracker = FindObjectOfType<QuestTracker>();

        if (questTracker != null)
        {
            questTracker.ResetQuestState();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }

        SceneManager.LoadScene("LandingPage");
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0f : 1f;
    }
}
