using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private EnemyAttribute _enemyAttribute;

    private Transform _player;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        _player = playerObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if(enemyToPlayerVector.magnitude <= _enemyAttribute.PlayerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }

    }
}
