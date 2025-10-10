using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelSceneController : MonoBehaviour
{
    //on invoke by player death
    //will restart level with fade animation

    [SerializeField] private float _sceneFadeDuration = 1f;
    private SceneFade _sceneFade;

    public float timer = 0f;

    private void Awake()
    {
        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneFade.FadeOutCoroutine(_sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    // 🧩 New method that checks if PersistentGameplay is already loaded
    public void LoadNextStage(string persistentScene, string sceneToBeUnloaded, string additiveScene)
    {
        StartCoroutine(LoadNextStageCoroutine(persistentScene, sceneToBeUnloaded, additiveScene));
    }




    private IEnumerator LoadNextStageCoroutine(string persistentScene, string sceneToBeUnloaded, string additiveScene)
    {
        // 1. Fade out before switching scenes
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(_sceneFadeDuration));

        Debug.Log($"Unloading old scene: {sceneToBeUnloaded}");
        yield return SceneManager.UnloadSceneAsync(sceneToBeUnloaded);


        Debug.Log($"Loading next additive scene: {additiveScene}");
        yield return SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);

        // 5. Set the newly loaded scene as active
        Scene nextScene = SceneManager.GetSceneByName(additiveScene);
        SceneManager.SetActiveScene(nextScene);
        Debug.Log($"Active scene set to: {nextScene.name}");

        // 7. Fade back in
        yield return StartCoroutine(_sceneFade.FadeInCoroutine(_sceneFadeDuration));

        Debug.Log($"Finished loading {additiveScene}");
    }


}
