using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlKeyHighlight : MonoBehaviour
{
    public KeyCode keyToHighlight;
    public Image keyImage;
    public TMP_Text keyText;

    private Color normalBackground = Color.white;
    private Color normalText = Color.black;

    private Color pressedBackground = Color.black;
    private Color pressedText = Color.white;

    private void Update()
    {
        bool pressed = Input.GetKey(keyToHighlight);

        if (pressed)
        {
            keyImage.color = pressedBackground;
            keyText.color = pressedText;
        }
        else
        {
            keyImage.color = normalBackground;
            keyText.color = normalText;
        }
    }
}
