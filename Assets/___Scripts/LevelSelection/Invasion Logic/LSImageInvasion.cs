using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LSImageInvasion : MonoBehaviour
{
    [Header("Village Settings")]
    [SerializeField] private string villageId;

    private Image uiImage;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

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
        {
            HandleInvasion();
        }

    }

    private void HandleInvasion()
    {
        if (!gameObject.scene.name.Contains("LevelSelector"))
            return;

        if (string.IsNullOrEmpty(villageId))
            return;

        if (LSManager.Instance.GetVillageState(villageId) == VillageState.Invaded)
            HandleInvasionLogic();
        else
        {
            HandleNonInvasionLogic();
        }


    }

    private void HandleInvasionLogic()
    {
        Color c = uiImage.color;
        c.a = 1f;
        uiImage.color = c;

    }
    private void HandleNonInvasionLogic()
    {
        Color c = uiImage.color;
        c.a = 0f;
        uiImage.color = c;

    }
}
