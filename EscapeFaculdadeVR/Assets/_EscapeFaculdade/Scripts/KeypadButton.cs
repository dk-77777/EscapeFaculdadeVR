using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public enum ButtonType
    {
        Digit,
        Submit,
        Clear
    }

    [SerializeField] private KeypadController keypad;
    [SerializeField] private ButtonType buttonType = ButtonType.Digit;
    [SerializeField] private string digit = "1";

    public void Press()
    {
        if (keypad == null)
            return;

        switch (buttonType)
        {
            case ButtonType.Digit:
                keypad.PressDigit(digit);
                break;
            case ButtonType.Submit:
                keypad.Submit();
                break;
            case ButtonType.Clear:
                keypad.Clear();
                break;
        }
    }
}
