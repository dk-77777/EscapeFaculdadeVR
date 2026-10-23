using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [Header("Porta")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float duration = 0.7f;
    [SerializeField] private bool startsOpen = false;

    [Header("Eventos")]
    public UnityEvent onOpened;
    public UnityEvent onClosed;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine movementRoutine;
    private bool isOpen;

    private void Awake()
    {
        if (doorPivot == null)
            doorPivot = transform;

        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        isOpen = startsOpen;
        doorPivot.localRotation = startsOpen ? openRotation : closedRotation;
    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    public void Toggle()
    {
        SetOpen(!isOpen);
    }

    private void SetOpen(bool open)
    {
        if (movementRoutine != null)
            StopCoroutine(movementRoutine);

        movementRoutine = StartCoroutine(RotateDoor(open));
    }

    private IEnumerator RotateDoor(bool open)
    {
        Quaternion start = doorPivot.localRotation;
        Quaternion target = open ? openRotation : closedRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);
            doorPivot.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        doorPivot.localRotation = target;
        isOpen = open;
        movementRoutine = null;

        if (isOpen)
            onOpened?.Invoke();
        else
            onClosed?.Invoke();
    }
}
