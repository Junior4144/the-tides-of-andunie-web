using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallsBehind : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    private HealthController _playerHealth;
    [SerializeField] private float deathDelay = 2f;

    private float _timer;

    private void Start()
    {
        _playerHealth = PlayerManager.Instance.GetComponentInChildren<HealthController>();
        _timer = deathDelay;
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Level 0"))
            enabled = false;
    }


    void Update()
    {
        if (!Camera.main) return;
        
        _camera = Camera.main;

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return; 
        }


        float rightEdge = DetermineCameraRightBorder();

        if (_playerHealth != null && transform.position.x > rightEdge)
        {
            _playerHealth.TakeDamage(1000f);
            enabled = false;
        }
            
    }

    public float DetermineCameraRightBorder() =>
        _camera.transform.position.x + (_camera.orthographicSize * _camera.aspect);
}
