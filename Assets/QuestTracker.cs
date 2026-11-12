using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestTracker : MonoBehaviour
{
    public bool questAccepted = false;
    public bool questCompleted = false;
    public bool questHandedIn = false;

    public bool hasKey = false;

    private static QuestTracker instance;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetQuestState()
    {
        questAccepted = false;
        questCompleted = false;
        hasKey = false;
    }

}

