using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelSceneController : MonoBehaviour
{
    public static RestartLevelSceneController Instance { get; private set; }

    [SerializeField] private float _sceneFadeDuration = 1f;
    private SceneFade _sceneFade;

    public float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    public void LoadNextStage(string sceneToBeUnloaded)
    {
        StartCoroutine(LoadNextStageCoroutine(sceneToBeUnloaded));
    }

    private IEnumerator LoadNextStageCoroutine(string additiveScene)
    {
        Debug.Log("Restart Scene Starting");
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

        Debug.Log($"Unloading old scene: {additiveScene}");
        yield return SceneManager.UnloadSceneAsync(additiveScene);

        Debug.Log($"Loading next additive scene: {additiveScene}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;

        Scene nextScene = SceneManager.GetSceneByName(additiveScene);
        SceneManager.SetActiveScene(nextScene);
        Debug.Log($"Active scene set to: {nextScene.name}");

        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

        Debug.Log($"Finished loading {additiveScene}");
    }
}
