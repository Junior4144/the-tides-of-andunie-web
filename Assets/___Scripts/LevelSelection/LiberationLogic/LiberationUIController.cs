using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LiberationUIController : MonoBehaviour
{
    public GameObject targetObject;

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
            StartCoroutine(HandleLiberationLogic());
    }

    private IEnumerator HandleLiberationLogic()
    {
        if (GlobalStoryManager.Instance.HasExitedLiberation)
        {
            yield return new WaitForSeconds(.5f);
            targetObject.SetActive(true);
            UIEvents.DefaultPopUPActive?.Invoke();

            string id = GlobalStoryManager.Instance.LastLiberatedVillageID;
            VillageNameText.text = LSManager.Instance.GetVillageName(id);

            GlobalStoryManager.Instance.SetBool("playLSInvasionCutscene", false);
        }
    }

    public void OnButtonClicked()
    {
        if (targetObject != null)
        {
            UIEvents.DefaultPopUpDisabled.Invoke();
            targetObject.SetActive(false);
        }
    }
}