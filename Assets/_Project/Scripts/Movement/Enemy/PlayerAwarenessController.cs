using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{   
    [SerializeField] private PirateAttributes _pirateAttributes;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.gameObject.transform;

        if (_playerTransform == null)
        {
            Debug.LogError("No game object with tag 'Player' found in the scene. Defaulting to self transform.");
            _playerTransform = transform;
        }
    }

    Vector2 _vectorToPlayer => _playerTransform.position - transform.position;
    public Vector2 DirectionToPlayer => _vectorToPlayer.normalized;
    
}
