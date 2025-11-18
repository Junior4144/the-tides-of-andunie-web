using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionLockController : MonoBehaviour
{
    [SerializeField] private GameObject Hyarrorstar_Region1;
    [SerializeField] private GameObject Hyarnustar_Region2;
    [SerializeField] private GameObject Andustar_Region3;
    [SerializeField] private GameObject Forostar_Region4;

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        Hyarrorstar_Region1.SetActive(false);
        Hyarnustar_Region2.SetActive(false);
        Andustar_Region3.SetActive(false);
        Forostar_Region4.SetActive(false);

        if (newScene == gameObject.scene)
            HandleCheckingRegions();
    }

    private void HandleCheckingRegions()
    {
        if (LSRegionLockManager.Instance._hyarrostarLocked)
        {
            Hyarrorstar_Region1.SetActive(true);
        }

        if (LSRegionLockManager.Instance._hyarnustarLocked)
        {
            Hyarnustar_Region2.SetActive(true);
        }

        if (LSRegionLockManager.Instance._andustarLocked)
        {
            Andustar_Region3.SetActive(true);
        }

        if (LSRegionLockManager.Instance._forostarLocked)
        {
            Forostar_Region4.SetActive(true);
        }
    }
}
