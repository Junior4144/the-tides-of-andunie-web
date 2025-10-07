using UnityEngine;
using UnityEngine.AI;

public class PirateMovement : MonoBehaviour
{
    [SerializeField] GameObject target;

    NavMeshAgent agent;

    [SerializeField] PriateAttributes _attributes;
    private Rigidbody2D _rigidbody;

    public float awarenessDistance = 10f;
    public float stopDistance = 2f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance <= awarenessDistance)
        {
            RotateTowardsTarget();
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.ResetPath();
        }

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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessDistance);
    }

}