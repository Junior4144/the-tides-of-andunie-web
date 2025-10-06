using UnityEngine;
using UnityEngine.AI;

public class VillagerMovement : MonoBehaviour
{
    [SerializeField] GameObject target;
    NavMeshAgent agent;
    private Vector2 _targetDirection;
    [SerializeField] VillagerAttributes _attributes;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _targetDirection = transform.up;
        agent.SetDestination(target.transform.position);

    }

    private void Update()
    {

        _targetDirection = transform.up;
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
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ForestTrees"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}