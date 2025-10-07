using UnityEngine;

public class VillagerTargetController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private VillagerAttributes _villagerAttribute;

    private void Awake()
    {
        
        if (_target == null)
        {
            Debug.LogError("No game object with tag 'Player' found in the scene. Defaulting to self transform.");
            _target = transform;
        }
    }

    Vector2 _vectorToTarget => _target.position - transform.position;
    public Vector2 DirectionToTarget => _vectorToTarget.normalized;
    public bool AwareOfTarget => _vectorToTarget.magnitude <= _villagerAttribute.TargetAwarenessDistance;

}
