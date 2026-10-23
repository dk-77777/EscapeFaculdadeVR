using UnityEngine;
using UnityEngine.Events;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float turnSpeed = 6f;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool loop = false;
    public UnityEvent onFinished;

    private int index;
    private bool playing;

    private void Start()
    {
        playing = playOnStart;
    }

    private void Update()
    {
        if (!playing || waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[index];
        Vector3 direction = target.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            index++;

            if (index >= waypoints.Length)
            {
                if (loop)
                    index = 0;
                else
                {
                    playing = false;
                    onFinished?.Invoke();
                }
            }
        }
    }

    public void Play()
    {
        index = 0;
        playing = true;
    }

    public void Stop()
    {
        playing = false;
    }
}
