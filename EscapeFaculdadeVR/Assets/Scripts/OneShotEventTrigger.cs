using UnityEngine;
using UnityEngine.Events;

public class OneShotEventTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnlyOnce = true;
    public UnityEvent onPlayerEntered;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnlyOnce && triggered)
            return;

        if (!other.CompareTag(playerTag) && !other.transform.root.CompareTag(playerTag))
            return;

        triggered = true;
        onPlayerEntered?.Invoke();
    }

    public void ResetTrigger()
    {
        triggered = false;
    }
}
