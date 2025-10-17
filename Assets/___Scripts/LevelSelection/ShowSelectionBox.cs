using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowSelectionBox : MonoBehaviour
{
    [SerializeField] private GameObject _BoxPrefab;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private float _offsetY = 3f;
    [SerializeField] private Camera _camera;

    private GameObject _boxInstance;

    private void OnEnable()
    {
        LevelSelection.OnPlayerEnterSelectionZone += ShowBox;
        LevelSelection.OnPlayerExitSelectionZone += HideBox;
        LevelSelection.OnPlayerLeavingLevelSelectionZone += DestroyBox;
        SceneStateManager.OnNonPersistentSceneActivated += HandleSceneLocationChange;
    }

    private void OnDisable()
    {
        LevelSelection.OnPlayerEnterSelectionZone -= ShowBox;
        LevelSelection.OnPlayerExitSelectionZone -= HideBox;
        LevelSelection.OnPlayerLeavingLevelSelectionZone -= DestroyBox;
        SceneStateManager.OnNonPersistentSceneActivated -= HandleSceneLocationChange;
    }

    private void Start()
    {
        if (_camera == null)
            _camera = GameManager.Instance.MainCamera;

        _boxInstance = Instantiate(_BoxPrefab, GetNewBubblePosition(), Quaternion.identity);
        _boxInstance.SetActive(false);


        Canvas canvas = _boxInstance.GetComponentInChildren<Canvas>();
        if (canvas != null && _camera != null)
            canvas.worldCamera = _camera;

    }


    private Vector3 GetNewBubblePosition() =>
    new Vector3(transform.position.x + _offsetX, transform.position.y + _offsetY, 0f);

    private void Update()
    {
        if (_boxInstance.activeSelf)
            _boxInstance.transform.position = GetNewBubblePosition();
    }

    public void ShowBox()
    {
        if (_boxInstance == null) return;
        _boxInstance.SetActive(true);
        Debug.Log("Showing box above player.");
    }

    public void HideBox()
    {
        if (_boxInstance == null) return;
        _boxInstance.SetActive(false);
        Debug.Log("Hiding box above player.");
    }
    public void DestroyBox()
    {
        Destroy(_boxInstance);
    }

    private void HandleSceneLocationChange() => SceneManager.MoveGameObjectToScene(_boxInstance, SceneManager.GetActiveScene());
}
