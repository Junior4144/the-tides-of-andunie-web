using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LiberationUIController : MonoBehaviour, IPointerClickHandler
{
    public GameObject targetObject;


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
        if(LSManager.Instance.GetVillageState("Village1") == VillageState.Liberated_Done && !GlobalStoryManager.Instance.Village1Liberated)
        {
            yield return new WaitForSeconds(2f);
            targetObject.SetActive(true);
            GlobalStoryManager.Instance.Village1Liberated = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}