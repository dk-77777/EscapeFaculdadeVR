using UnityEngine;

public class ProfessorChaseSimple : MonoBehaviour
{
    [Header("Alvo")]
    [SerializeField] private Transform player;

    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 3.2f;
    [SerializeField] private float turnSpeed = 6f;
    [SerializeField] private float catchDistance = 1.2f;
    [SerializeField] private bool keepSameHeight = true;

    [Header("Estado")]
    [SerializeField] private bool chaseOnStart = false;

    private bool chasing;

    private void Start()
    {
        chasing = chaseOnStart;
    }

    private void Update()
    {
        if (!chasing || player == null)
            return;

        Vector3 targetPosition = player.position;
        if (keepSameHeight)
            targetPosition.y = transform.position.y;

        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        if (distance <= catchDistance)
        {
            chasing = false;
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
            return;
        }

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void StartChase()
    {
        chasing = true;
    }

    public void StopChase()
    {
        chasing = false;
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}
