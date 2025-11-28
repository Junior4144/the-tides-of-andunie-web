using System.Collections;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _tutorialPrefab;
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private Transform _prefabPanel;
    [SerializeField] private float initialDelay = 0.4f;

    private ScaleOnEnable _scaleOnEnable;

    private void Awake()
    {
        _scaleOnEnable = GetComponentInChildren<ScaleOnEnable>(true);
    }

    private void OnEnable()
    {
        UIEvents.OnTutorialDeactivated += DeactivateCanvas;
    }

    private void OnDisable()
    {
        UIEvents.OnTutorialDeactivated -= DeactivateCanvas;
    }

    private void Start()
    {
        StartCoroutine(InitalSetup());
    }

    private IEnumerator InitalSetup()
    {
        yield return new WaitForSeconds(initialDelay);

        _canvas.SetActive(true);
        Instantiate(_questPrefab, _prefabPanel);
        Instantiate(_tutorialPrefab, _prefabPanel);
        yield return new WaitForSeconds(initialDelay);

        Time.timeScale = 0f;

        UIEvents.OnTutorialActive?.Invoke();
    }

    public void OnOkayPressed()
    {
        UIEvents.OnTutorialDeactivated?.Invoke();
    }

    public void DeactivateCanvas()
    {
        StartCoroutine(CloseTutorial());
    }

    private IEnumerator CloseTutorial()
    {
        Time.timeScale = 1f;

        _scaleOnEnable.HideWithScale();
        yield return new WaitForSeconds(.8f);

        _canvas.SetActive(false);
    }
}
