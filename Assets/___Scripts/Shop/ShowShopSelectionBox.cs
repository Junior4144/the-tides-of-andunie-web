using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowShopSelectionBox : MonoBehaviour
{
    [SerializeField] private GameObject _BoxPrefab;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private Camera _camera;

    private GameObject _boxInstance;

    public static ShowSelectionBox Instance;

    private void OnEnable()
    {
        LSShopController.OnPlayerEnterSelectionZone += ShowBox;
        LSShopController.OnPlayerExitSelectionZone += HideBox;
        SceneStateManager.OnNonPersistentSceneActivated += HandleSceneLocationChange;
    }

    private void OnDisable()
    {
        LSShopController.OnPlayerEnterSelectionZone -= ShowBox;
        LSShopController.OnPlayerExitSelectionZone -= HideBox;
        SceneStateManager.OnNonPersistentSceneActivated -= HandleSceneLocationChange;
    }

    private void Start()
    {
        if (_camera == null)
            _camera = GameManager.Instance.MainCamera;

        _boxInstance = Instantiate(_BoxPrefab, GetNewBubblePosition(), Quaternion.identity);
        _boxInstance.SetActive(false);

        Debug.Log("STARTING SHOW SELECTION BOX");
        Canvas canvas = _boxInstance.GetComponentInChildren<Canvas>();
        if (canvas != null && _camera != null)
            canvas.worldCamera = _camera;
    }


    private Vector3 GetNewBubblePosition() => new Vector3(PlayerManager.Instance.GetPlayerTransform().position.x + _offsetX, PlayerManager.Instance.GetPlayerTransform().position.y + _offsetY, 0f);

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

    private void HandleSceneLocationChange() => SceneManager.MoveGameObjectToScene(_boxInstance, SceneManager.GetActiveScene());
}
