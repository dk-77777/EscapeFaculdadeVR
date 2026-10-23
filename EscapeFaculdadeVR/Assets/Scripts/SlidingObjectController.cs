using System.Collections;
using UnityEngine;

public class SlidingObjectController : MonoBehaviour
{
    [SerializeField] private Transform movingPart;
    [SerializeField] private Vector3 localOpenOffset = new Vector3(0f, 0f, 0.35f);
    [SerializeField] private float duration = 0.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Coroutine routine;

    private void Awake()
    {
        if (movingPart == null)
            movingPart = transform;

        closedPosition = movingPart.localPosition;
        openPosition = closedPosition + localOpenOffset;
    }

    public void Open()
    {
        MoveTo(openPosition);
    }

    public void Close()
    {
        MoveTo(closedPosition);
    }

    private void MoveTo(Vector3 target)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        Vector3 start = movingPart.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);
            movingPart.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        movingPart.localPosition = target;
        routine = null;
    }
}
