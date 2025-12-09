using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockRegionUIController : MonoBehaviour
{
    [SerializeField] private GameObject _regionUIObject;

    public TextMeshProUGUI VillageNameText;

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            StartCoroutine(HandleRegionCheck());
    }

    private IEnumerator HandleRegionCheck()
    {
        if (GlobalStoryManager.Instance.HasExitedLiberation)
        {
            yield return new WaitForSeconds(.2f);
            Debug.Log("UnlockRegionUIController attempting to show region UI");
            Region unlockedRegion = LSRegionLockManager.Instance.CheckForNewUnlockedRegion();
            Debug.Log("UnlockRegionUIController current region unlocked " + unlockedRegion);
            if (unlockedRegion != Region.None)
            {
                Debug.Log("New region unlocked: " + unlockedRegion);
                _regionUIObject.SetActive(true);
                UIEvents.DefaultPopUPActive?.Invoke();
            }
        }
    }

    public void OnButtonClicked()
    {
        if (_regionUIObject != null)
        {
            UIEvents.DefaultPopUpDisabled.Invoke();
            _regionUIObject.SetActive(false);
        }
    }
}
