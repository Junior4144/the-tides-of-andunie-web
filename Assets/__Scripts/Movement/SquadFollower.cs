using UnityEngine;

public class SquadFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.01f;
    [SerializeField] private bool faceMovementDirection = true;
   
    
    private Transform hero;
    private Rigidbody2D rb;
    
    private Vector3 formationOffsetLocal;
    private float formationAngleLocal;
    private Vector3 targetPositionInFormation;
    private Impulse impulseScript;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseScript = GetComponentInChildren<Impulse>();
        
        if (transform.parent != null && transform.parent.parent != null)
        {
            hero = transform.parent.parent;

            Vector3 worldOffset = transform.position - hero.position;
            formationOffsetLocal = Quaternion.Inverse(hero.rotation) * worldOffset;
            formationAngleLocal = transform.eulerAngles.z - hero.eulerAngles.z;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Could not find hero (parent's parent)");
        }
    }
    
    void LateUpdate()
    {
        if (hero == null) return;
        
        Vector3 rotatedOffset = hero.rotation * formationOffsetLocal;
        targetPositionInFormation = hero.position + rotatedOffset;
    }
    
    void FixedUpdate()
    {
        if (hero == null) return;
        
        if (impulseScript != null && impulseScript.IsInImpulse()) return;
            MoveTowardsFormationPosition();
        
        if (!faceMovementDirection)
            RotateTowardsFormationAngle();
    }
    
    private void MoveTowardsFormationPosition()
    {
        Vector3 directionToTarget = targetPositionInFormation - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        
        if (distanceToTarget > stoppingDistance)
        {
            rb.linearVelocity = directionToTarget.normalized * moveSpeed * distanceToTarget;
            
            if (faceMovementDirection)
                LookAtTarget(directionToTarget);
        }
        else
        {
            transform.position = targetPositionInFormation;
            rb.linearVelocity = Vector2.zero;
            
            RotateTowardsFormationAngle();
        }
    }
    
    private void LookAtTarget(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = targetRotation;
        }
    }
    
    private void RotateTowardsFormationAngle()
    {
        float targetRotation = hero.eulerAngles.z + formationAngleLocal;
        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
    }
}