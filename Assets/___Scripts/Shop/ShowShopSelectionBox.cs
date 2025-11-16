using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowShopSelectionBox : MonoBehaviour
{
    [SerializeField] private GameObject _BoxPrefab;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;

    private Camera _camera;
    private GameObject _boxInstance;
    private Canvas canvas;

    private void OnEnable()
    {
        LSShopController.OnPlayerEnterSelectionZone += ShowBox;
        LSShopController.OnPlayerExitSelectionZone += HideBox;
        SceneStateManager.OnNonPersistentSceneActivated += HandleSceneLocationChange;

        SceneManager.activeSceneChanged += HandleCheck;
    }

    private void OnDisable()
    {
        LSShopController.OnPlayerEnterSelectionZone -= ShowBox;
        LSShopController.OnPlayerExitSelectionZone -= HideBox;
        SceneStateManager.OnNonPersistentSceneActivated -= HandleSceneLocationChange;

        SceneManager.activeSceneChanged -= HandleCheck;
    }

    private void Update()
    {
        if (_boxInstance != null && _boxInstance.activeSelf && PlayerManager.Instance != null)
            _boxInstance.transform.position = GetNewBubblePosition();
    }

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
        {
            HandleBoxSetup();
        }
    }

    private void HandleBoxSetup()
    {
        _camera = CameraManager.Instance.GetCamera();
        _boxInstance = Instantiate(_BoxPrefab, GetNewBubblePosition(), Quaternion.identity);
        _boxInstance.SetActive(false);

        canvas = _boxInstance.GetComponentInChildren<Canvas>();
        canvas.worldCamera = _camera;
    }

    private Vector3 GetNewBubblePosition()
    {
        return new Vector3(PlayerManager.Instance.GetPlayerTransform().position.x + 
            _offsetX, PlayerManager.Instance.GetPlayerTransform().position.y + 
            _offsetY, 0f);

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
