using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private float _deactivationDelay = 0.06f;
    [SerializeField] private GameObject _tutorialPrefab;
    [SerializeField] private Transform _prefabPanel;

    private void Start()
    {
        InstantiateTutorialPrefab();
    }

    private void InstantiateTutorialPrefab()
    {
        if (_tutorialPrefab != null)
            Instantiate(_tutorialPrefab, _prefabPanel);
        else
            Debug.Log("TutorialUIController could not find either Tutorial Prefab");
    }

    public void DeactivateCanvas()
    {
        StartCoroutine(DeactivateAfterDelay(_deactivationDelay));
    }

    private IEnumerator DeactivateAfterDelay(float deactivationDelay)
    {
        yield return new WaitForSeconds(deactivationDelay);

        _canvas.SetActive(false);
    }
}
