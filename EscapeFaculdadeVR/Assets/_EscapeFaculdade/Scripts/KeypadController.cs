using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KeypadController : MonoBehaviour
{
    [Header("Codigo")]
    [SerializeField] private string correctCode = "4362";
    [SerializeField] private int maxDigits = 4;

    [Header("Tela")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private string emptyText = "----";

    [Header("Eventos")]
    public UnityEvent onCorrectCode;
    public UnityEvent onWrongCode;

    private string currentInput = "";
    private bool solved = false;

    private void Start()
    {
        RefreshDisplay();
    }

    public void PressDigit(string digit)
    {
        if (solved || string.IsNullOrEmpty(digit))
            return;

        if (currentInput.Length >= maxDigits)
            return;

        currentInput += digit;
        RefreshDisplay();
    }

    public void Submit()
    {
        if (solved)
            return;

        if (currentInput == correctCode)
        {
            solved = true;
            if (displayText != null)
                displayText.text = "OK";
            onCorrectCode?.Invoke();
        }
        else
        {
            onWrongCode?.Invoke();
            Clear();
        }
    }

    public void Clear()
    {
        if (solved)
            return;

        currentInput = "";
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (displayText == null)
            return;

        if (string.IsNullOrEmpty(currentInput))
            displayText.text = emptyText;
        else
            displayText.text = currentInput.PadRight(maxDigits, '-');
    }
}
