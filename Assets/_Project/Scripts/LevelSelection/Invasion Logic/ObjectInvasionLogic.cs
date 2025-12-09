using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectInvasionLogic : MonoBehaviour
{
    [SerializeField] GameObject GameObject;
    public string villageId;

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
            HandleInvasionLogic();
    }

    private void HandleInvasionLogic()
    {
        if(LSManager.Instance.GetVillageState(villageId) == VillageState.Invaded)
            GameObject.SetActive(true);
        else
            GameObject.SetActive(false);

    }

}
