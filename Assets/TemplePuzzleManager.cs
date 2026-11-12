using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplePuzzleManager : MonoBehaviour
{
    public SpriteRenderer switch1;
    public SpriteRenderer switch2;
    public SpriteRenderer switch3;

    public GameObject slidingDoor;
    public SceneTransition doorTransition;

    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    public float slideDistance = -2f;
    public float slideDuration = 1f;

    private bool switch1Activated = false;
    private bool switch2Activated = false;
    private bool switch3Activated = false;
    private bool puzzleComplete = false;

    private void Start()
    {
        if (doorTransition != null)
            doorTransition.enabled = false;
    }

    public void ToggleSwitch(int switchNumber)
    {
        if (puzzleComplete)
            return;

        if (switchNumber == 2)
        {
            ResetPuzzle();
            return;
        }

        if (switchNumber == 1)
        {
            switch1Activated = !switch1Activated;
            switch1.color = switch1Activated
                ? activeColor : inactiveColor;
        }
        else if (switchNumber == 3)
        {
            switch3Activated = !switch3Activated;
            switch3.color = switch3Activated
                ? activeColor : inactiveColor;
        }

        CheckPuzzle();
    }

    private void CheckPuzzle()
    {
        if (switch1Activated &&
            !switch2Activated &&
            switch3Activated)
        {
            puzzleComplete = true;
            StartCoroutine(OpenDoor());
        }
    }

    private void ResetPuzzle()
    {
        switch1Activated = false;
        switch2Activated = false;
        switch3Activated = false;

        switch1.color = inactiveColor;
        switch2.color = inactiveColor;
        switch3.color = inactiveColor;
    }

    private IEnumerator OpenDoor()
    {
        if (slidingDoor == null)
            yield break;

        Vector3 startPosition = slidingDoor.transform.position;
        Vector3 endPosition = startPosition +
            new Vector3(slideDistance, 0f, 0f);

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;

            slidingDoor.transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                Mathf.Clamp01(elapsed / slideDuration)
            );

            yield return null;
        }

        slidingDoor.transform.position = endPosition;

        if (doorTransition != null)
            doorTransition.enabled = true;
    }
}