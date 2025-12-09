using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    private Camera _camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[CameraManager] Duplicate instance detected, destroying this one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null)
                Debug.LogError("[CameraManager] No Camera assigned and no MainCamera found!");
        }
    }
    private void OnEnable() =>
        GameManager.OnGameStateChanged += HandleGameStateChanged;

    private void Start() =>
        HandleGameStateChanged(GameManager.Instance.CurrentState);

    private void HandleGameStateChanged(GameState newState)
    {
    }
    public Camera GetCamera()
    {
        return _camera;
    }
}
