using TMPro;
using UnityEngine;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;

    public void SetObjective(string newText)
    {
        if (textField != null)
            textField.text = newText;
    }

    public void Hide()
    {
        if (textField != null)
            textField.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (textField != null)
            textField.gameObject.SetActive(true);
    }
}
