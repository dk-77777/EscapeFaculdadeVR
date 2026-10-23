using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    public UnityEvent onDelayFinished;

    public void Play()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return new WaitForSeconds(delay);
        onDelayFinished?.Invoke();
    }
}
