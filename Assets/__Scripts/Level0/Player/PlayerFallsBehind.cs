using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallsBehind : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    private IHealthController _playerHealth;
    [SerializeField] private float deathDelay = 2f;

    private float _timer;

    private IEnumerator Start()
    {
        yield return null;

        _playerHealth = PlayerManager.Instance.GetComponentInChildren<IHealthController>();
        _timer = deathDelay;

        Debug.Log($"[PlayerFallsBehind] Current Scene: {SceneManager.GetActiveScene().name}");
        if(SceneManager.GetActiveScene().name != "Level 0")
            enabled = false;
    }


    void Update()
    {
        if (!Camera.main) return;

        _camera = CameraManager.Instance.GetComponent<Camera>();

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
