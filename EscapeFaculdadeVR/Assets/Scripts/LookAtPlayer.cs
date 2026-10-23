using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private bool onlyY = true;

    private void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 target = player.position;
        if (onlyY)
            target.y = transform.position.y;

        Vector3 direction = target - transform.position;
        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }
}
