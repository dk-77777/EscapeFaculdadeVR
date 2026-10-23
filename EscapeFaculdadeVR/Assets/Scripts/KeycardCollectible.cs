using UnityEngine;
using UnityEngine.Events;

public class KeycardCollectible : MonoBehaviour
{
    [SerializeField] private bool destroyAfterCollect = true;
    public UnityEvent onCollected;

    private bool collected;

    public void Collect()
    {
        if (collected)
            return;

        collected = true;

        if (GameManager.Instance != null)
            GameManager.Instance.CollectCard();

        onCollected?.Invoke();

        if (destroyAfterCollect)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
