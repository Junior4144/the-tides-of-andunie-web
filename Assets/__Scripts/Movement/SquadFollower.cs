using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SquadFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.01f;
    [SerializeField] private float rotationSpeed = 360f;


    private Transform squad;
    private Rigidbody2D rb;

    private Vector3 formationOffsetLocal;
    private float formationAngleLocal;
    private Vector3 targetPositionInFormation;
    private Impulse impulseScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseScript = GetComponentInChildren<Impulse>();

        if (transform.parent != null)
        {
            squad = transform.parent;

            Vector3 worldOffset = transform.position - squad.position;
            formationOffsetLocal = Quaternion.Inverse(squad.rotation) * worldOffset;
            formationAngleLocal = transform.eulerAngles.z - squad.eulerAngles.z;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Could not find parent object: Squad");
        }
    }

    void Update()
    {
        if (squad == null) return;
        if (impulseScript != null && impulseScript.IsInImpulse()) return;

        MoveTowardsFormationPosition();
    }
    
    void LateUpdate()
    {
        if (squad == null) return;

        // Calculate the target world position based on the squad's current position and rotation
        Vector3 unitOffsetFromSquadCenter = squad.rotation * formationOffsetLocal;
        targetPositionInFormation = squad.position + unitOffsetFromSquadCenter;
    }

    private void MoveTowardsFormationPosition()
    {
        Vector3 directionToTarget = targetPositionInFormation - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        MatchFormationAngle();

        if (distanceToTarget > stoppingDistance)
        {
            SetVelocity(directionToTarget.normalized * moveSpeed * distanceToTarget);
        }
        else
        {
            SetVelocity(Vector2.zero);
            // Optional: You could snap the position here, but letting the velocity stop is often smoother.
            // MatchFormationPosition(); 
        }
    }

    // THIS METHOD WAS THE PROBLEM AND HAS BEEN REMOVED
    // private void RotateTowardsFormationPosition(Vector3 direction)
    // {
    //     if (direction != Vector3.zero)
    //         SetRotation(targetAngle: Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    // }

    private void MatchFormationAngle() =>
        SetRotation(targetAngle: squad.eulerAngles.z + formationAngleLocal);

    private void MatchFormationPosition() =>
         transform.position = targetPositionInFormation;

    private void SetRotation(float targetAngle)
    {
        float currentAngle = rb.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        rb.SetRotation(newAngle);
    }
        
    private void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;
}