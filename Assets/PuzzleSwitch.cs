using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public TemplePuzzleManager puzzleManager;
    public int switchNumber;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (puzzleManager != null)
        {
            puzzleManager.ToggleSwitch(switchNumber);
        }
    }
}
