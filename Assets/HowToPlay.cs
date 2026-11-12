using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public GameObject howToPlayPanel;

    private void Start()
    {
        howToPlayPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        howToPlayPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        howToPlayPanel.SetActive(false);
    }
}
