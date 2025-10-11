using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    private Camera _camera;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }
    private void OnEnable() =>
        // Subscribe to GameManager event
        GameManager.OnGameStateChanged += HandleGameStateChanged;

    private void Start() =>
        HandleGameStateChanged(GameManager.Instance.CurrentState);

    private void HandleGameStateChanged(GameState newState)
    {
        Debug.Log($"CameraManager responding to new state: {newState}");

        if (newState == GameState.Cutscene)
            _camera.gameObject.SetActive(false);
        else
            _camera.gameObject.SetActive(true);


    }
}
