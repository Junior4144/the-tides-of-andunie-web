using UnityEngine;
using UnityEngine.AI;

public class VillagerMovement : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    [SerializeField] VillagerAttributes _attributes;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(target.transform.position);
        RotateTowardsTarget();
    }
    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(_rigidbody.rotation, targetRotation.eulerAngles.z, _attributes.RotationSpeed * Time.deltaTime)
        );
    }

}