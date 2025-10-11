using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerManager : MonoBehaviour
{
    [SerializeField] private float _sceneFadeDuration;
    public static SceneControllerManager Instance { get; private set; }
    private SceneFade _sceneFade;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    public void LoadNextStage(string sceneToBeUnloaded, string additiveScene)
    {
        StartCoroutine(LoadNextStageCoroutine(sceneToBeUnloaded, additiveScene));
    }
    private IEnumerator LoadNextStageCoroutine(string sceneToBeUnloaded, string additiveScene)
    {
        Debug.Log("Next Scene Change Starting");
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

        Debug.Log($"UnloadSceneAsync: {sceneToBeUnloaded}");
        yield return SceneManager.UnloadSceneAsync(sceneToBeUnloaded);

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




        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

         Debug.Log("Completed Scene Change");
    }

}
