using UnityEngine;
using UnityEngine.Events;

public class ExitTerminal : MonoBehaviour
{
    public UnityEvent onAccessGranted;
    public UnityEvent onAccessDenied;

    private bool granted;

    public void TryOpenExit()
    {
        if (granted)
            return;

        if (GameManager.Instance != null && GameManager.Instance.HasAllCards)
        {
            granted = true;
            onAccessGranted?.Invoke();
        }
        else
        {
            onAccessDenied?.Invoke();
        }
    }
}
