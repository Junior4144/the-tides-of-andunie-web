using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    [SerializeField]
    private EnemyAttribute _enemyAttribute;

    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
    }

    public Vector2 DirectionToPlayer => (_playerTransform.position - transform.position).normalized;
    public bool AwareOfPlayer => (_playerTransform.position - transform.position).magnitude <= _enemyAttribute.PlayerAwarenessDistance;
    
}
