using System;
using UnityEngine;

public class PlayerFallsBehind : MonoBehaviour
{
    Camera _camera;
    [SerializeField]
    private HealthController _playerHealth;

    private void Start() =>
        _playerHealth = GetComponent<HealthController>();
    void Update()
    {
        if (!Camera.main) return;
        
        _camera = Camera.main;

        float rightEdge = DetermineCameraRightBorder();

        if (_playerHealth != null && transform.position.x > rightEdge)
            _playerHealth.TakeDamage(10f);
    }

    public float DetermineCameraRightBorder() =>
        _camera.transform.position.x + (_camera.orthographicSize * _camera.aspect);
}
