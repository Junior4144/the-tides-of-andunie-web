using System;
using UnityEngine;

public class PlayerFallsBehind : MonoBehaviour
{
    Camera _camera;
    [SerializeField]
    private EnemyAttribute _enemyAttribute;
    private HealthController _playerHealth;
    private void Start()
    {
        _playerHealth = GetComponent<HealthController>();
    }
    void Update()
    {
        _camera = Camera.main;
        float rightEdge = _camera.transform.position.x + (_camera.orthographicSize * _camera.aspect);

        if (transform.position.x > rightEdge)
        {
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(_enemyAttribute.DamageAmount);
            }
        }
    }
}
