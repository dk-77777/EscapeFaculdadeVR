using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private float minDelay = 0.05f;
    [SerializeField] private float maxDelay = 0.25f;
    [SerializeField] private bool flickerOnStart = false;

    private Coroutine routine;

    private void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (flickerOnStart)
            StartFlicker();
    }

    public void StartFlicker()
    {
        if (routine == null)
            routine = StartCoroutine(FlickerRoutine());
    }

    public void StopFlicker()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = null;

        if (targetLight != null)
            targetLight.enabled = true;
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            if (targetLight != null)
                targetLight.enabled = !targetLight.enabled;

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
